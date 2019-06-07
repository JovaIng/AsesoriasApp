using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Entidades;
using Negocio;
using System.Collections.Generic;
using System.Text;

namespace AsesoriasApp
{
    [Activity(Label = "Asesores disponibles")]
    public class AsesoresMateriasActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AsesoresMateriasActivity);

            ListView lsvAsesores = FindViewById<ListView>(Resource.Id.lsvAsesores);
            PublicacionesDALC gruposAsesoriasDALC = new PublicacionesDALC(PackageName, this, this);
            UsuariosDALC usuariosDALC = new UsuariosDALC(PackageName, this, this);
            List<PublicacionEntity> lstGruposAsesoria = gruposAsesoriasDALC.MostrarGruposAsesoria(Variables.MateriaSeleccionada, Variables.ModoUsuario);
            List<string> lstGruposAsesoriasString = new List<string>();
            StringBuilder stringBuilder = null;
            foreach (PublicacionEntity grupo in lstGruposAsesoria)
            {
                UsuarioEntity usuario = usuariosDALC.MostrarUsuario(grupo.IdAsesor);
                if(usuario != null)
                {
                    if (!string.IsNullOrEmpty(usuario.Correo))
                    {
                        stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine(string.Format("{0} {1}", usuario.Nombres, usuario.Apellidos).Trim());
                        stringBuilder.AppendLine(usuario.Carrera);
                        stringBuilder.AppendLine(grupo.DescripciónCurso);
                        stringBuilder.AppendLine(grupo.TipoCobro);
                        lstGruposAsesoriasString.Add(stringBuilder.ToString());
                    }
                }
            }

            if (lstGruposAsesoriasString != null && lstGruposAsesoriasString.Count > 0)
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstGruposAsesoriasString.ToArray());
                lsvAsesores.Adapter = adapter;
            }

            Button btnCreaAsesoria = FindViewById<Button>(Resource.Id.btnCreaAsesoria);
            btnCreaAsesoria.Click += delegate
            {
                Intent intent = new Intent(ApplicationContext, typeof(PublicarActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            };
        }
    }
}