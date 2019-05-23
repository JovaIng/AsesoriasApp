using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Entidades;
using Negocio;

namespace AsesoriasApp
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login_activity);

            EditText edtUsuario = FindViewById<EditText>(Resource.Id.edtUsuario);
            EditText edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            Button btnEntrar = FindViewById<Button>(Resource.Id.btnEntrar);

            btnEntrar.Click += delegate {
                if (ValidaCampos(edtUsuario, edtPassword))
                {
                    UsuariosDALC usuariosDALC = new UsuariosDALC(PackageName, this, this);
                    UsuarioEntity usuario = usuariosDALC.MostrarUsuario(edtUsuario.Text, edtPassword.Text);
                    if (!string.IsNullOrEmpty(usuario.Nombre))
                    {
                        Intent intent = new Intent(ApplicationContext, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Finish();
                    }
                    else
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Usuario no encontrado");
                        builder.SetCancelable(false);
                        builder.SetMessage("No se encontró ningún usuario con la información proporcionada.");
                    }
                }
            };
        }

        private bool ValidaCampos(EditText edtUsuario, EditText edtPassword)
        {
            bool success = true;
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            
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
                if (!correo.Contains("@") || !correo.Contains(".com"))
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
                builder.SetTitle("Campos requeridos");
                builder.SetMessage(stringBuilder.ToString());
                RunOnUiThread(() => { builder.Show(); });
            }

            return success;
;        }
    }
}