using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSocketsXamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    /*****Funciona Tambien******/
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Button boton1 = FindViewById<Button>(Resource.Id.button1);
            EditText mensajeAEnviar = FindViewById<EditText>(Resource.Id.mensaje);
            EditText mensajesRecibidos = FindViewById<EditText>(Resource.Id.mensajesRecibidos);

            boton1.Click += async (sender, e) =>
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>{
                    {"type","mensaje"},
                    {"contenido",mensajeAEnviar.Text}
                };
                string jsonObj = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                ClienteWebSocketsFuncionaTambien.enviarMensaje(jsonObj);
                mensajesRecibidos.Text += mensajeAEnviar.Text + "\n";
            };

            ClienteWebSocketsFuncionaTambien.iniciarCliente("ws://192.168.43.251:8000");
            ClienteWebSocketsFuncionaTambien.myEventHandlerRecibirMensaje += (sender, e) =>
            {
                mensajesRecibidos.Text += e.mensaje.usuario + ": " + e.mensaje.contenido + "\n";
            };
        }

        protected override void OnStop()
        {
            base.OnStop();
            Dictionary<string, string> dictionary = new Dictionary<string, string>{
                {"type","evento"},
                {"accion","salir"}
            };
            string jsonObj = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            ClienteWebSocketsFuncionaTambien.enviarMensaje(jsonObj);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }

    /*****Funciona******/
    /*public class MainActivity : AppCompatActivity
    {        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Button boton1 = FindViewById<Button>(Resource.Id.button1);
            EditText mensajeAEnviar = FindViewById<EditText>(Resource.Id.mensaje);
            EditText mensajesRecibidos = FindViewById<EditText>(Resource.Id.mensajesRecibidos);

            boton1.Click += async (sender, e) =>
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>{
                    {"type","mensaje"},
                    {"contenido",mensajeAEnviar.Text}
                };
                string jsonObj = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
                ClienteWebSocketsFunciona.enviarMensaje(jsonObj);
                
            };

            ClienteWebSocketsFunciona.iniciarCliente("ws://192.168.1.73:8000");
            ClienteWebSocketsFunciona.myEventHandlerRecibirMensaje += (sender, e) =>
            {
                mensajesRecibidos.Text += e.mensaje + "\n";
            };

        }

        protected override void OnStop()
        {
            base.OnStop();
            Dictionary<string, string> dictionary = new Dictionary<string, string>{
                {"type","evento"},
                {"accion","salir"}
            };
            string jsonObj = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            ClienteWebSocketsFunciona.enviarMensaje(jsonObj);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
    }*/
}

//https://www.newtonsoft.com/json
//https://www.newtonsoft.com/json/help/html/SerializeDictionary.htm "Serialize a Dictionary"
//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/ "The Task asynchronous programming model in C#"
//https://csharp.h