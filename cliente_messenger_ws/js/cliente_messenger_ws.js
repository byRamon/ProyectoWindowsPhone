//var mysocket = new WebSocket("wss://echo.websocket.org");
	var mysocket;
	var mensajeEnviar;
	var nombreUsuario;
	var ipServidor = "ws://192.168.1.67:8000";//"ws://192.168.43.103:8000"; //
	
	var fueUltimoMensajeRecivido=false;
	var fueUltimoMensajeEnviado=false;
	
	function clickEnviarMensaje()
	{		
		var jsonObj = JSON.stringify({type:'mensaje',		
		contenido:mensajeEnviar.value});		
		var clase;
		mysocket.send(jsonObj);			
		console.log("Mensaje enviado: " + jsonObj);
		if(fueUltimoMensajeEnviado)
			clase = "div-mensaje-enviado-par";					
		else{
			clase = "div-mensaje-enviado";
			fueUltimoMensajeEnviado=true;
		}		
		nuevoMensajeRecibidoOEnviado(divPanelMensajes,clase,nombreUsuario.value,mensajeEnviar.value);
		fueUltimoMensajeRecivido=false;
		//listaDeClientes.innerHTML("option",hola);
	}

	function clickDesconectar()
	{
		var jsonObj = JSON.stringify({type:'evento',
		accion:'salir'});		
		mysocket.send(jsonObj);		
		//mysocket.close();
	}
	
	function clickConectar(ev)
	{
		mensajeEnviar=document.getElementById("msgEnviar");	
	    nombreUsuario = document.getElementById("nombreUsuario");
		mysocket = new WebSocket(ipServidor);
		
		mysocket.onopen = function(ev)
		{
			//mensajeEnviar.value = "Socket abierto";
			console.log("Socket abierto");
			var jsonObj = JSON.stringify(
			{type:'nuevo_usuario',
			nombre:nombreUsuario.value});
			mysocket.send(jsonObj);
		}
			
		mysocket.onclose = function(ev)
		{			
			console.log("Socket cerrado de forma satisfatoria");
		}

		mysocket.onerror = function(ev)
		{
			console.log("Error" + ev.data);
		}

		mysocket.onmessage = function(ev)
		{
			var data = JSON.parse(ev.data);
			if (data['type']=='mensaje'){
				console.log(data['nombre_usuario'] + " dice:" + data['contenido']);
				if(fueUltimoMensajeRecivido)
					clase = "div-mensaje-recibido-par";					
				else{
					clase = "div-mensaje-recibido";
					fueUltimoMensajeRecivido=true;
				}		
				nuevoMensajeRecibidoOEnviado(divPanelMensajes,clase,data['nombre_usuario'],data['contenido']);
				fueUltimoMensajeEnviado=false;
			}
			else if (data['type']=='evento')
				if (data['accion']=='entrar')
					nuevoUsuarioConectado(divUsuarios,data['nombre_usuario'])							
		}		
	}