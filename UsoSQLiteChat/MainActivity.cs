using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using UsoSQLiteChat.Modelo;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Android.Views;

namespace UsoSQLiteChat
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private string _usuario;
        private List<Mensaje> lstMensajes;
        private string pathBaseDeDatos;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Android.Content.ISharedPreferences preferences = GetSharedPreferences("PreferenciasUsuario", Android.Content.FileCreationMode.Private);
            string preferenciaUsuario = preferences.GetString("Usuario", "");
            bool usuarioEntro = Mostrarlogin(preferenciaUsuario);
            pathBaseDeDatos = Path.Combine(FilesDir.AbsolutePath, "mensajes.sqlite");
            File.Delete(pathBaseDeDatos);
            lstMensajes = new List<Mensaje>();
            using (var connection = new SQLiteConnection(pathBaseDeDatos))
            {
                connection.DropTable<Mensaje>();
                connection.CreateTable<Mensaje>();
                lstMensajes = connection.Table<Mensaje>().ToList();
                connection.Close();
                connection.Dispose();
            }
            ListView lvMensajes = FindViewById<ListView>(Resource.Id.lvMensajes);
            lvMensajes.Adapter = new RowMensaje(this, lstMensajes);
            lvMensajes.ItemLongClick += ShowMenu;

            Button btnMensaje = FindViewById<Button>(Resource.Id.btnUsuario);
            btnMensaje.Click += (sender, e) =>
            {
                EditText txtUsuario = FindViewById<EditText>(Resource.Id.txtUsuario);
                if (!string.IsNullOrEmpty(txtUsuario.Text))
                {
                    _usuario = txtUsuario.Text;
                    Mostrarlogin(txtUsuario.Text);
                }
            };
        }

        private void ShowMenu(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            ListView lvMensajes = FindViewById<ListView>(Resource.Id.lvMensajes);
            PopupMenu mnu = new PopupMenu(this, lvMensajes);
            mnu.MenuInflater.Inflate(Resource.Menu.PopMenu, mnu.Menu);
            mnu.MenuItemClick += (s, arg) =>
            {
                Toast.MakeText(this, lstMensajes[e.Position].mensaje, ToastLength.Short).Show();
            };
        }

        private void IniciarClienteMensajeria()
        {
            ListView lvMensajes = FindViewById<ListView>(Resource.Id.lvMensajes);
            Button btnMensaje = FindViewById<Button>(Resource.Id.btnMensaje);
            EditText mensajeAEnviar = FindViewById<EditText>(Resource.Id.txtMensaje);
            ClienteWebSocketsFuncionaTambien.iniciarCliente("ws://192.168.1.67:8000", _usuario);
            btnMensaje.Click += async (sender, e) =>
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>{
                    {"type","mensaje"},
                    {"contenido",mensajeAEnviar.Text}
                };
                string jsonObj = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                ClienteWebSocketsFuncionaTambien.enviarMensaje(jsonObj);
                mensajeAEnviar.Text = "";
            };

            ClienteWebSocketsFuncionaTambien.myEventHandlerRecibirMensaje += (sender, e) =>
            {
                Mensaje obj = new Mensaje();
                obj.fechaEnvio = DateTime.Now;
                obj.recibido = _usuario != e.mensaje.usuario;
                obj.usuario = e.mensaje.usuario;
                obj.mensaje = e.mensaje.contenido;
                lstMensajes.Add(obj);
                using (var connection = new SQLiteConnection(pathBaseDeDatos))
                {
                    connection.Insert(obj);
                    connection.Close();
                    connection.Dispose();
                }
                lvMensajes.Adapter = new RowMensaje(this, lstMensajes);
            };
        }

        private bool Mostrarlogin(string usuario)
        {
            bool usuarioEntro = usuario.Length > 0;
            var viewLogin = FindViewById<LinearLayout>(Resource.Id.llUsuario);
            var viewMensajes = FindViewById<ListView>(Resource.Id.lvMensajes);
            var llMensaje = FindViewById<LinearLayout>(Resource.Id.llMensaje);
            viewLogin.Visibility = usuarioEntro ? Android.Views.ViewStates.Gone : Android.Views.ViewStates.Visible;
            viewMensajes.Visibility = usuarioEntro ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
            llMensaje.Visibility = usuarioEntro ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
            if (usuarioEntro)
            {
                _usuario = usuario;
                IniciarClienteMensajeria();
            }
            return usuarioEntro;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.opciones, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.btnSalir)
            {
                ClienteWebSocketsFuncionaTambien.Salir(_usuario);
                Mostrarlogin("");
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnStop()
        {
            base.OnStop();
            Android.Content.ISharedPreferences preferences = GetSharedPreferences("PreferenciasUsuario", Android.Content.FileCreationMode.Private);
            Android.Content.ISharedPreferencesEditor editor = preferences.Edit();
            EditText usuario = FindViewById<EditText>(Resource.Id.txtUsuario);
            string preferenciaUsuario = usuario.Text;
            editor.PutString("Usuario", preferenciaUsuario);
            editor.Commit();
        }
    }
}