using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Entidades;
using Negocio;

namespace AsesoriasApp
{
    [Activity(Label = "")]
    public class PublicacionesActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PublicacionesActivity);
            Button btnPublicar = FindViewById<Button>(Resource.Id.btnCreaAsesoria);

            ListView lsvAsesores = FindViewById<ListView>(Resource.Id.lsvAsesores);
            PublicacionesDALC publicacionesDALC = new PublicacionesDALC(PackageName, this, this);
            UsuariosDALC usuariosDALC = new UsuariosDALC(PackageName, this, this);

            string modo = string.Empty;
            if (Variables.ModoUsuario.Equals(Variables.MODO_ALUMNO))
            {
                modo = Variables.MODO_ASESOR;
                SetTitle(Resource.String.titulo_publicacion_asesores);
                btnPublicar.Text = "Solicitar asesoría";
            }
            else
            {
                modo = Variables.MODO_ALUMNO;
                SetTitle(Resource.String.titulo_publicacion_alumnos);
                btnPublicar.Text = "Publicar asesoría";
            }

            List<PublicacionEntity> lstPublicacionesEntitys = publicacionesDALC.MostrarPublicaciones(Variables.MateriaSeleccionada, modo);
            List<string> lstPublicacionesString = new List<string>();
            StringBuilder stringBuilder = null;
            foreach (PublicacionEntity publicacion in lstPublicacionesEntitys)
            {
                UsuarioEntity usuario = usuariosDALC.MostrarUsuario(publicacion.IdUsuario);
                if (usuario != null)
                {
                    if (!string.IsNullOrEmpty(usuario.Correo))
                    {
                        stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(string.Format("{0} {1}", usuario.Nombres, usuario.Apellidos).Trim());
                        //stringBuilder.AppendLine("" + usuario.Carrera);
                        stringBuilder.AppendLine(publicacion.DescripciónCurso);
                        stringBuilder.AppendLine("Cobro: " + publicacion.TipoCobro);
                        if(publicacion.Modo.Equals(Variables.MODO_ASESOR))
                            stringBuilder.AppendLine("Nivel " + usuario.Nivel);
                        lstPublicacionesString.Add(stringBuilder.ToString());
                    }
                }
            }

            if (lstPublicacionesString != null && lstPublicacionesString.Count > 0)
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstPublicacionesString.ToArray());
                lsvAsesores.Adapter = adapter;
            }

            btnPublicar.Click += delegate
            {
                Intent intent = new Intent(ApplicationContext, typeof(PublicarActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            };
        }

       /* public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.btnRegresar: RunOnUiThread(() => { Finish(); });
                    break;

            }
            return base.OnOptionsItemSelected(item);
        }*/
    }
}