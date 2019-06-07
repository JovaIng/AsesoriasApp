using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V7.App;
using AlertDialog = Android.App.AlertDialog;
using Negocio;
using Entidades;
using Android.Content;

namespace AsesoriasApp
{
    [Activity(Label = "Materias", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MateriasActivity);

            ListView lsvMaterias = FindViewById<ListView>(Resource.Id.lsvMaterias);

            MateriasDALC materiasDALC = new MateriasDALC(PackageName, this, this);
            List<MateriaEntity> lstMateriasEntity = materiasDALC.MostrarMaterias();
            List<string> lstMaterias = new List<string>();
            foreach(MateriaEntity materia in lstMateriasEntity)
                lstMaterias.Add(materia.Nombre);
               
            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstMaterias.ToArray());
            lsvMaterias.Adapter = adapter;

            lsvMaterias.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle(lstMaterias[e.Position]);
                builder.SetMessage("¿Qué le gustaría ser en esta materia?");
                builder.SetCancelable(false);
                builder.SetPositiveButton("Asesor", delegate {
                    RunOnUiThread(() => { Toast.MakeText(this, "Seleccionó asesor", ToastLength.Long).Show(); });
                    builder.Dispose();
                    Variables.MateriaSeleccionada = lstMaterias[lsvMaterias.SelectedItemPosition];
                    Variables.ModoUsuario = Variables.MODO_ASESOR;
                    Intent intent = new Intent(ApplicationContext, typeof(PublicarActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();

                });
                builder.SetNegativeButton("Alumno", delegate {
                    RunOnUiThread(() => { Toast.MakeText(this, "Seleccionó alumno", ToastLength.Long).Show(); });
                    Variables.MateriaSeleccionada = lstMaterias[lsvMaterias.SelectedItemPosition];
                    Variables.ModoUsuario = Variables.MODO_ALUMNO;
                    builder.Dispose();
                    Intent intent = new Intent(ApplicationContext, typeof(PublicarActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    StartActivity(intent);
                });
                RunOnUiThread(() => { builder.Show(); });
            };
        }
    }
}