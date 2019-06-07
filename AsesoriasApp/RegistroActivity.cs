
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Entidades;
using Negocio;

namespace AsesoriasApp
{
    [Activity(Label = "Registro", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RegistroActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistroActivity);

            EditText edtNombres = FindViewById<EditText>(Resource.Id.edtNombres);
            EditText edtApellidos = FindViewById<EditText>(Resource.Id.edtApellidos);
            EditText edtCorreo = FindViewById<EditText>(Resource.Id.edtCorreo);
            EditText edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            EditText edtTelefono = FindViewById<EditText>(Resource.Id.edtTelefono);

            edtTelefono.InputType = Android.Text.InputTypes.ClassNumber;
            edtPassword.InputType = Android.Text.InputTypes.TextVariationPassword;
            edtCorreo.InputType = Android.Text.InputTypes.TextVariationEmailAddress;

            List<string> lstString = new List<string>();
            lstString.Add("- SELECCIONE FACULTAD -");
            lstString.Add("FACULTAD DE INGENIERÍA, ARQUITECTURA Y DISEÑO");
            lstString.Add("FACULTAD DE CIENCIAS MARINAS");
            lstString.Add("FACULTAD DE CIENCIAS");

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.SpinnerItem, lstString.ToArray());
            Spinner spnFacultad = FindViewById<Spinner>(Resource.Id.spnFacultad);
            spnFacultad.Adapter = adapter;

            lstString = new List<string>();
            lstString.Add("- SELECCIONE SEMESTRE -");

            for (int i = 1; i <= 10; i++)
                lstString.Add("SEMESTRE " + i);

            adapter = new ArrayAdapter<string>(this, Resource.Layout.SpinnerItem, lstString.ToArray());
            Spinner spnSemestre = FindViewById<Spinner>(Resource.Id.spnSemestre);
            spnSemestre.Adapter = adapter;

            Button btnRegistrarse = FindViewById<Button>(Resource.Id.btnRegistrar);
            btnRegistrarse.Click += delegate {
                if(ValidaCampos(edtNombres, edtApellidos, edtCorreo, edtPassword, edtTelefono, spnSemestre, spnFacultad))
                {
                    UsuarioEntity usuario = new UsuarioEntity();
                    usuario.Nombres = edtNombres.Text;
                    usuario.Apellidos = edtApellidos.Text;
                    usuario.Correo = edtCorreo.Text;
                    usuario.Password = edtPassword.Text;
                    usuario.Telefono = edtTelefono.Text;
                    usuario.Semestre = spnSemestre.SelectedItemPosition.ToString();
                    usuario.Facultad = spnFacultad.Adapter.GetItem(spnFacultad.SelectedItemPosition).ToString();

                    UsuariosDALC usuariosDALC = new UsuariosDALC(PackageName, this, this);
                    if (usuariosDALC.InsertarUsuario(usuario))
                    {
                        RunOnUiThread(() => { Toast.MakeText(this, "usuario creado :)", ToastLength.Short).Show(); });
                        Intent intent = new Intent(ApplicationContext, typeof(LoginActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Finish();
                        Variables.CorreoUsuario = usuario.Correo;
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
            stringBuilder.AppendLine(string.Empty);

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
                if (!edtCorreo.Text.Contains("@uabc.edu.mx"))
                {
                    count++;
                    success = false;
                    stringBuilder.AppendLine(string.Format("{0} - El formato del correo es incorrecto. Sólo se admiten correos uabc.", count));
                }
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

            if (spnSemestre.SelectedItemPosition <= 0)
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Indique el semestre que cursa.", count));
            }

            if (spnFacultad.SelectedItemPosition <= 0)
            {
                count++;
                success = false;
                stringBuilder.AppendLine(string.Format("{0} - Indique la facultad en la que cursa.", count));
            }

            if (!success)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetCancelable(false);
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