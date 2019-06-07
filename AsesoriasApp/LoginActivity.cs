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
    [Activity(Label = "Iniciar sesión", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]

    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginActivity);

            EditText edtUsuario = FindViewById<EditText>(Resource.Id.edtUsuario);
            EditText edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            Button btnEntrar = FindViewById<Button>(Resource.Id.btnEntrar);
            Button btnRegistrarse = FindViewById<Button>(Resource.Id.btnRegistrarse);

            btnRegistrarse.Click += delegate {
                Intent intent = new Intent(ApplicationContext, typeof(RegistroActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            };

            btnEntrar.Click += delegate {
                if (ValidaCampos(edtUsuario, edtPassword))
                {
                    UsuariosDALC usuariosDALC = new UsuariosDALC(PackageName, this, this);
                    UsuarioEntity usuario = usuariosDALC.MostrarUsuario(edtUsuario.Text, edtPassword.Text);
                    if (!string.IsNullOrEmpty(usuario.Nombres))
                    {
                        Variables.Usuario = usuario;
                        Intent intent = new Intent(ApplicationContext, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    }
                    else
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetCancelable(false);
                        builder.SetTitle("Usuario no encontrado");
                        builder.SetCancelable(false);
                        builder.SetMessage("No se encontró ningún usuario con la información proporcionada.");
                        builder.SetPositiveButton("Aceptar", delegate { builder.Dispose(); });
                        RunOnUiThread(() => { builder.Show(); });
                    }
                }
            };
        }

        private bool ValidaCampos(EditText edtUsuario, EditText edtPassword)
        {
            bool success = true;

            int count = 0;
            StringBuilder stringBuilder = new StringBuilder("Antes de comenzar corrija lo siguiente:");
            if (string.IsNullOrEmpty(edtUsuario.Text))
            {
                count++;
                stringBuilder.AppendLine(string.Format("{0} - El correo es requerido.", count));
                success = false;
            }
            else
            {
                string correo = edtUsuario.Text.ToLower();
                if (!correo.Contains("@uabc.edu.mx"))
                {
                    count++;
                    stringBuilder.AppendLine(string.Format("{0} - El formato del correo no es correcto.", count));
                    success = false;
                }
            }

            if (string.IsNullOrEmpty(edtPassword.Text))
            {
                count++;
                stringBuilder.AppendLine(string.Format("{0} - La contraseña es requerida.", count));
                success = false;
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
;        }
    }
}