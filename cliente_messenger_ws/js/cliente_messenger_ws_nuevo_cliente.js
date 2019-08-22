var divUsuarios = document.getElementById("div-usuarios");
var divPanelMensajes = document.getElementById("div-panel-mensajes");

//nuevoUsuarioConectado(divUsuarios,"Luis")
//nuevoMensajeRecibidoOEnviado(divPanelMensajes,"div-mensaje-recibido-par","Jose","Hola");

function nuevoUsuarioConectado(divUsuariosConectados,nombreUsuario){
	var divNuevoUsuarioConectado = document.createElement("div");
	divNuevoUsuarioConectado.className = "div-usuario-conectado";
	
	var nuevoCanvas = document.createElement("canvas");
	nuevoCanvas.className="canvas-usuario-conectado";
	var myContext = nuevoCanvas.getContext("2d");
	divNuevoUsuarioConectado.appendChild(nuevoCanvas);
	drawCircleInCanvas(myContext);
	
	var nuevoh3 = document.createElement("h3");
	nuevoh3.innerText = nombreUsuario;
	divNuevoUsuarioConectado.appendChild(nuevoh3);
	
	divUsuariosConectados.appendChild(divNuevoUsuarioConectado);	
}

function drawCircleInCanvas(context)
{
	context.beginPath();
	context.fillStyle = "green";
	context.strokeStyle = "black";
	context.arc(150, 80, 60, 0, 2 * Math.PI,true);
	context.fill();
	context.stroke();
	context.closePath();
}

function nuevoMensajeRecibidoOEnviado(divPanelMenajes,clase,nombreUsuario,mensaje)
{
	var divNuevoMensajeRecibido = document.createElement("div");
	divNuevoMensajeRecibido.className = clase;
	
	var nuevoh4NombreUsuario = document.createElement("h4");
	nuevoh4NombreUsuario.className="h4-nombre-usuario";	
	nuevoh4NombreUsuario.innerHTML = nombreUsuario;
	divNuevoMensajeRecibido.appendChild(nuevoh4NombreUsuario);	
	
	var nuevoh3MensajeUsuario = document.createElement("h3");
	nuevoh3MensajeUsuario.className = "h3-mensaje-usuario";
	nuevoh3MensajeUsuario.innerText = mensaje;
	divNuevoMensajeRecibido.appendChild(nuevoh3MensajeUsuario);
	
	divPanelMenajes.appendChild(divNuevoMensajeRecibido);
}