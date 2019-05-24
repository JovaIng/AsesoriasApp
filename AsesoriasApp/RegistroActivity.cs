
using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace AsesoriasApp
{
    [Activity(Label = "RegistroActivity")]
    public class RegistroActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registro_activity);

            EditText edtNombres = FindViewById<EditText>(Resource.Id.edtNombres);
            EditText edtApellidos = FindViewById<EditText>(Resource.Id.edtApellidos);
            EditText edtCorreo = FindViewById<EditText>(Resource.Id.edtCorreo);
            EditText edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            EditText edtTelefono = FindViewById<EditText>(Resource.Id.edtTelefono);
            Spinner spnSemestre = FindViewById<Spinner>(Resource.Id.spnSemestre);
            Spinner spnFacultad = FindViewById<Spinner>(Resource.Id.spnFacultad);
            Button btnRegistrarse = FindViewById<Button>(Resource.Id.btnRegistrar);

            btnRegistrarse.Click += delegate {
                if(ValidaCampos(edtNombres, edtApellidos, edtCorreo, edtPassword, edtTelefono, spnSemestre, spnFacultad))
                {
                    // Registrar usuario con la información proporcionada.
                    if (true)
                    {
                        RunOnUiThread(() => { Toast.MakeText(this, "usuario creado :)", ToastLength.Short).Show(); });
                        Intent intent = new Intent(ApplicationContext, typeof(LoginActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Finish();

                    }
                    
                }
            };
        }

        private bool ValidaCampos(EditText edtNombres, EditText edtApellidos, EditText edtCorreo, EditText edtPassword, 
            EditText edtTelefono, Spinner spnSemestre, Spinner spnFacultad)
        {
            int count = 0;
            bool success = true;
            StringBuilder stringBuilder = new StringBuilder("Por favor corrija los siguientes detalles: ");

            if (string.IsNullOrEmpty(edtNombres.Text))
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Es necesarios que indique sus nombres.", count));
            }

            if (string.IsNullOrEmpty(edtApellidos.Text))
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Es necesarios que indique sus apellidos.", count));
            }

            if (string.IsNullOrEmpty(edtCorreo.Text))
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Es necesarios que indique sus correo.", count));
            }
            else
            {
                if (!edtCorreo.Text.Contains("@"))
                {
                    count++;
                    success = false;
                }
                else
                {
                    if (!edtCorreo.Text.Contains(".com"))
                    {
                        count++;
                        success = false;
                    }
                }

                if(!success)
                    stringBuilder.AppendLine(string.Format("{0} - El formato del correo es incorrecto.", count));
            }

            if (string.IsNullOrEmpty(edtPassword.Text))
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Es necesarios que indique una contraseña.", count));
            }

            if (string.IsNullOrEmpty(edtTelefono.Text))
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Es necesarios que indique su teléfono.", count));
            }
            else
            {
                if (edtTelefono.Text.Length < 10)
                    stringBuilder.AppendLine(string.Format("{0} - El número de teléfono debe ser de al menos 10 dígito.", count));
            }

            if (!success)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Campos requeridos");
                builder.SetMessage(stringBuilder.ToString());
                builder.SetPositiveButton("Aceptar", delegate {
                    builder.Dispose();
                });

                RunOnUiThread(() => { builder.Show(); });
            }

            return success;
        }
    }
}