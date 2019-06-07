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
using Android.Views;

namespace AsesoriasApp
{
    [Activity(Label = "Materias", ScreenOrientation = ScreenOrientation.Portrait)]
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
            if (lstMateriasEntity != null && lstMateriasEntity.Count == 0)
            {
                lstMaterias.Add("PROBABILIDAD Y ESTADÍSTICA");
                lstMaterias.Add("QUIMICA GENERAL");
                lstMaterias.Add("CALCULO DIFERENCIAL");
                lstMaterias.Add("PROGRAMACIÓN BÁSICA");
                lstMaterias.Add("PROGRAMACIÓN ESTRUCTURADA");
                lstMaterias.Add("ELECTRICIDAD Y MAGNETISMO");
                lstMaterias.Add("CALCULO INTEGRAL");
                lstMaterias.Add("MATEMATICAS AVANZADAS");
                lstMaterias.Add("MÉTODOS NUMÉRICOS");
                lstMaterias.Add("CIRCUITOS I");
                lstMaterias.Add("ESTÁTICA");
                lstMaterias.Add("ALGORITMOS Y ESTRUCTURAS DE DATOS");
                lstMaterias.Add("CIRCUITOS DIGITALES");
                lstMaterias.Add("PROGRAMACIÓN ORIENTADA A OBJETOS");
                lstMaterias.Add("INTELIGENCIA ARTIFICIAL");
                lstMaterias.Add("LENGUAJES DECLARATIVOS");
                lstMaterias.Sort();

                foreach (string materia in lstMaterias)
                    materiasDALC.InsertarMaterias(materia);
            }
            else
            {
                foreach (MateriaEntity materia in lstMateriasEntity)
                    lstMaterias.Add(materia.Nombre);
            }
            
            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.SpinnerItem, lstMaterias.ToArray());
            lsvMaterias.Adapter = adapter;

            lsvMaterias.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                Variables.MateriaSeleccionada = lstMaterias[e.Position];
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle(lstMaterias[e.Position]);
                builder.SetMessage("¿Qué le gustaría ser en esta materia?");
                builder.SetCancelable(false);
                builder.SetPositiveButton("Asesor", delegate {
                    builder.Dispose();
                    RunOnUiThread(() => {
                        Toast.MakeText(this, "Seleccionó asesor", ToastLength.Long).Show();
                        Variables.ModoUsuario = Variables.MODO_ASESOR;
                        Intent intent = new Intent(ApplicationContext, typeof(PublicacionesActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    });
                });
                builder.SetNegativeButton("Alumno", delegate {
                    builder.Dispose();
                    RunOnUiThread(() => {
                        Toast.MakeText(this, "Seleccionó alumno", ToastLength.Long).Show();
                        Variables.ModoUsuario = Variables.MODO_ALUMNO;
                        builder.Dispose();
                        Intent intent = new Intent(ApplicationContext, typeof(PublicacionesActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    });
                });
                RunOnUiThread(() => { builder.Show(); });
            };
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