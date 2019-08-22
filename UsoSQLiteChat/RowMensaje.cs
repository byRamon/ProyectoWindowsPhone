using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using UsoSQLiteChat.Modelo;

namespace UsoSQLiteChat
{
    class RowMensaje : BaseAdapter<Mensaje>
    {
        private Context _context;
        private List<Mensaje> _data;
        public RowMensaje(Context context, List<Mensaje> lst)
        {
            this._context = context;
            this._data = lst;
        }
        public override int Count
        {
            get { return _data.Count; }
        }
        public override Mensaje this[int position]
        {
            get { return _data[position]; }
        }

        public override long GetItemId(int position)
        {
            return _data[position].id;
        }
        public void updateList(List<Mensaje> data)
        {
            this._data = data;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Mensaje mensaje = _data[position];
            //if (convertView == null)
            //{
            var inflater = LayoutInflater.From(_context);
            if (mensaje.recibido)
            {
                convertView = inflater.Inflate(Resource.Layout.RowRecibido, parent, false);
                TextView txtUsuario = convertView.FindViewById<TextView>(Resource.Id.tvUsuarioRecibido);
                TextView txtMensaje = convertView.FindViewById<TextView>(Resource.Id.tvMensajeRecibido);
                txtUsuario.Text = mensaje.usuario;
                txtMensaje.Text = mensaje.mensaje;
            }
            else
            {
                convertView = inflater.Inflate(Resource.Layout.RowEnviado, parent, false);
                TextView txtUsuario = convertView.FindViewById<TextView>(Resource.Id.tvUsuarioEnviado);
                TextView txtMensaje = convertView.FindViewById<TextView>(Resource.Id.tvMensajeEnviado);
                txtUsuario.Text = position == 0 || (position > 0 && mensaje.usuario != _data[position - 1].usuario) ? mensaje.usuario : "";
                txtMensaje.Text = mensaje.mensaje;
            }
            //}
            //if (mensaje.recibido)
            //{
            //    TextView txtUsuario = convertView.FindViewById<TextView>(Resource.Id.tvUsuarioRecibido);
            //    if (txtUsuario == null)
            //    {
            //        var inflater = LayoutInflater.From(_context);
            //        convertView = inflater.Inflate(Resource.Layout.RowRecibido, parent, false);
            //    }
            //    txtUsuario = convertView.FindViewById<TextView>(Resource.Id.tvUsuarioRecibido);
            //    TextView txtMensaje = convertView.FindViewById<TextView>(Resource.Id.tvMensajeRecibido);
            //    txtUsuario.Text = mensaje.usuario;
            //    txtMensaje.Text = mensaje.mensaje;
            //}
            //else
            //{
            //    TextView txtUsuario = convertView.FindViewById<TextView>(Resource.Id.tvUsuarioEnviado);
            //    if (txtUsuario == null)
            //    {
            //        var inflater = LayoutInflater.From(_context);
            //        convertView = inflater.Inflate(Resource.Layout.RowEnviado, parent, false);
            //    }
            //    txtUsuario = convertView.FindViewById<TextView>(Resource.Id.tvUsuarioEnviado);
            //    TextView txtMensaje = convertView.FindViewById<TextView>(Resource.Id.tvMensajeEnviado);
            //    txtUsuario.Text = position == 0 || (position > 0 && mensaje.usuario != _data[position - 1].usuario) ? mensaje.usuario : "";
            //    txtMensaje.Text = mensaje.mensaje;
            //}
            convertView.ScrollY = convertView.Height;
            return convertView;
        }
    }
}