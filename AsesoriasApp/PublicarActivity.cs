
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Negocio;
using System;
using System.Collections.Generic;

namespace AsesoriasApp
{
    [Activity(Label = "CrearGrupoAsesoriaActivity")]
    public class PublicarActivity : Activity
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

            lstString.Add("SELECCIONE CAPACIDAD");
            for (int i = 1; i <= 20; i++)
                lstString.Add(string.Format("{0} ALUMNOS", i));

            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstString.ToArray());
            spnCapacidad.Adapter = adapter;

            lstString = new List<string>();
            lstString.Add("SELECCIONE COBRO");
            lstString.Add("ESPECIE");
            lstString.Add("EFECTIVO");
            lstString.Add("POR DEFINIR");

            adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstString.ToArray());
            spnTipoCobro.Adapter = adapter;

            Button btnCreaGrupo = FindViewById<Button>(Resource.Id.btnCreaGrupo);
            btnCreaGrupo.Click += delegate {
                if(ValidaCampos(edtDescripcion, spnCapacidad, spnTipoCobro))
                {
                    GrupoAsesoriaEntity entidad = new GrupoAsesoriaEntity();
                    entidad.Materia = Variables.MateriaSeleccionada;
                    entidad.DescripciónCurso = edtDescripcion.Text;
                    entidad.IdAsesor = Variables.Usuario.IdUsuario;
                    entidad.TipoCobro = spnTipoCobro.Adapter.GetItem(spnTipoCobro.SelectedItemPosition).ToString();
                    entidad.Capacidad = spnCapacidad.SelectedItemPosition;
                    PublicacionesDALC gruposAsesoriasDALC = new PublicacionesDALC(PackageName, this, this);
                    if (gruposAsesoriasDALC.InsertarGruposAsesorias(entidad))
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Registro exitoso");
                        builder.SetMessage("Su publicación como asesor ahora está disponible.");
                        builder.SetPositiveButton("Aceptar", delegate {
                            builder.Dispose();
                            Intent intent = new Intent(ApplicationContext, typeof(AsesoresMateriasActivity));
                            intent.SetFlags(ActivityFlags.NewTask);
                            StartActivity(intent);
                            Finish();
                        });
                        RunOnUiThread(() => { builder.Show(); });
                    }
                }
            };
        }

        private bool ValidaCampos(TextView edtDescripcion, Spinner spnCapacidad, Spinner spnTipoCobro)
        {
            return true;
        }
    }
}