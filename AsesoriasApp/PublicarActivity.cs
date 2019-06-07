
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Negocio;
using System.Collections.Generic;
using System.Text;
using AlertDialog = Android.App.AlertDialog;

namespace AsesoriasApp
{
    [Activity(Label = "CrearGrupoAsesoriaActivity")]
    public class PublicarActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PublicarActivity);

            TextView edtDescripcion = FindViewById<TextView>(Resource.Id.edtDescripcion);
            Spinner spnCapacidad = FindViewById<Spinner>(Resource.Id.spnCapacidad);
            Spinner spnTipoCobro = FindViewById<Spinner>(Resource.Id.spnTipoCobro);
            TextView txtMateria = FindViewById<TextView>(Resource.Id.txtMateria);

            txtMateria.Text = Variables.MateriaSeleccionada;

            List<string> lstString = new List<string>();
            lstString = new List<string>();
            lstString.Add("SELECCIONE COBRO");
            lstString.Add("ESPECIE");
            lstString.Add("EFECTIVO");
            lstString.Add("POR DEFINIR");

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstString.ToArray());
            spnTipoCobro.Adapter = adapter;

            if (Variables.ModoUsuario.Equals(Variables.MODO_ASESOR))
            {
                SetTitle(Resource.String.titulo_crear_grupo_asesoria);
                lstString = new List<string>();
                lstString.Add("SELECCIONE CAPACIDAD");
                for (int i = 1; i <= 20; i++)
                    lstString.Add(string.Format("{0} ALUMNOS", i));

                adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstString.ToArray());
                spnCapacidad.Adapter = adapter;
            }
            else
            {
                SetTitle(Resource.String.titulo_crear_solicitud_asesoria);
                TextView txtCapacidad = FindViewById<TextView>(Resource.Id.txtCapacidad);
                txtCapacidad.Visibility = Android.Views.ViewStates.Gone;
                spnCapacidad.Visibility = Android.Views.ViewStates.Gone;
            }

            Button btnCreaGrupo = FindViewById<Button>(Resource.Id.btnCreaGrupo);
            btnCreaGrupo.Click += delegate {
                if(ValidaCampos(edtDescripcion, spnCapacidad, spnTipoCobro))
                {
                    PublicacionEntity entidad = new PublicacionEntity();
                    entidad.Materia = Variables.MateriaSeleccionada;
                    entidad.DescripciónCurso = edtDescripcion.Text;
                    entidad.IdUsuario = Variables.Usuario.IdUsuario;
                    entidad.TipoCobro = spnTipoCobro.Adapter.GetItem(spnTipoCobro.SelectedItemPosition).ToString();

                    if (Variables.ModoUsuario.Equals(Variables.MODO_ASESOR))
                        entidad.Capacidad = spnCapacidad.SelectedItemPosition;
                    
                    entidad.Modo = Variables.ModoUsuario;
                    PublicacionesDALC publicacionesDALC = new PublicacionesDALC(PackageName, this, this);
                    if (publicacionesDALC.InsertarPublicacion(entidad))
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);

                        if (Variables.ModoUsuario.Equals(Variables.MODO_ASESOR))
                        {
                            builder.SetTitle("Registro exitoso");
                            builder.SetMessage("Su ha creado su grupo de asesoría correctamente.");
                        }
                        else
                            builder.SetTitle("Solicitud creada");
                        builder.SetMessage("Su solicitud de asesoría a sido creada correctamente.");

                        builder.SetPositiveButton("Aceptar", delegate {
                            builder.Dispose();
                            Finish();
                        });
                        RunOnUiThread(() => { builder.Show(); });
                    }
                }
            };
        }

        private bool ValidaCampos(TextView edtDescripcion, Spinner spnCapacidad, Spinner spnTipoCobro)
        {
            bool success = true;
            StringBuilder stringBuilder = new StringBuilder("Los siguientes campos son requeridos\n");

            int count = 0;
            if (string.IsNullOrEmpty(edtDescripcion.Text))
            {
                count++;
                stringBuilder.AppendLine(string.Format("{0} - Descripción", count));
            }

            if (Variables.ModoUsuario.Equals(Variables.MODO_ASESOR))
            {
                if (spnCapacidad.SelectedItemPosition <= 0)
                {
                    count++;
                    stringBuilder.AppendLine(string.Format("{0} - Capacidad", count));
                }
            }

            if (spnTipoCobro.SelectedItemPosition <= 0)
            {
                count++;
                stringBuilder.AppendLine(string.Format("{0} - Tipo de cobro", count));
            }

            if (count > 0)
            {
                success = false;
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetCancelable(false);
                builder.SetTitle("Campos requeridos");
                builder.SetMessage(stringBuilder.ToString());
                builder.SetPositiveButton("Aceptar", delegate { builder.Dispose(); });
                RunOnUiThread(() => { builder.Show(); });
            }

            return success;
        }

        /*public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.btnRegresar:
                    RunOnUiThread(() => { Finish(); });
                    break;

            }
            return base.OnOptionsItemSelected(item);
        }*/
    }
}