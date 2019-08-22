import websockets
import asyncio
import sys
import signal
import json
import logging

logging.basicConfig()

class Servidor:
    def __init__(self,host,port):
        self.USERS = set()                 

    async def escuchandoPeticiones(self,websocket,path):
        nombre_usuario = 'nombre_prueba'
        abierto = True
        print ("Conexión aceptada")
        #await self.register(websocket,nombre_usuario)
        try:
            #await websocket.send("Bienvenido!")
            async for message in websocket:
                data = json.loads(message)
                if data['type'] == 'nuevo_usuario':
                    nombre_usuario = data['nombre']
                    await self.register(websocket,nombre_usuario)#Nueva línea
                elif data['type'] == 'mensaje':
                    await self.notificar_mensaje_a_usuarios(data['contenido'],nombre_usuario)
                elif data['type'] == 'evento':
                    print(f"{nombre_usuario}: {data}")
                    if data['accion'] == 'salir':
                        #pass
                        abierto = False
                        await self.unregister(websocket,nombre_usuario,not abierto)                        
                        break
                else:
                    logging.error("evento no soportado: {}",
					data)
                print(f"{nombre_usuario}: {data}")
        finally:
            await self.unregister(websocket,nombre_usuario,abierto)
        
    async def register(self,websocket,nombre_usuario):
        self.USERS.add(websocket)
        await self.notificar_entrada_usuario(nombre_usuario)
    
    async def unregister(self,websocket,nombre_usuario,abierto):        
        if abierto:            
            print('eliminando socket')
            try:
                self.USERS.remove(websocket)
                await self.notificar_salida_usuario(nombre_usuario)
            finally:
                pass
                        
    async def notificar_entrada_usuario(self,nombre_usuario):
        if self.USERS:
            mensaje = self.estado_usuario('entrada_usuario',nombre_usuario)
            await asyncio.wait([user.send(mensaje) for user in self.USERS])
    
    async def notificar_salida_usuario(self,nombre_usuario):
        if self.USERS:
            mensaje = self.estado_usuario('salida_usuario',nombre_usuario)
            await asyncio.wait([user.send(mensaje) for user in self.USERS])
                           
    async def notificar_mensaje_a_usuarios(self,msj,nombre_usuario):
        if self.USERS:
            mensaje = self.mensaje_usuario(msj,nombre_usuario)
            await asyncio.wait([user.send(mensaje) for user in self.USERS])
    
    def estado_usuario(self,type,nombre_usuario):
        if type == 'entrada_usuario':
            return json.dumps({'type':'evento','accion':'entrar','nombre_usuario':nombre_usuario})
        elif type == 'salida_usuario':
            return json.dumps({'type':'evento','accion':'salir','nombre_usuario':nombre_usuario})

    def mensaje_usuario(self,msj,nombre_usuario):        
        return json.dumps({'type':'mensaje','contenido':msj,'nombre_usuario':nombre_usuario})                    


servidorIp = '192.168.1.67' #ip
servidor = Servidor(servidorIp,8000)

asyncio.get_event_loop().run_until_complete(websockets.serve(servidor.escuchandoPeticiones,servidorIp, 8000))
asyncio.get_event_loop().run_forever()