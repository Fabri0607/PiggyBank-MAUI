<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
</head>
<body>

<h1>PiggyBank - Frontend</h1>

<p><strong>Repositorio del Backend:</strong> <a href="https://github.com/Fabri0607/PiggyBank">PiggyBank</a></p>

<h2>📘 Overview</h2>
<p>
  PiggyBank es una aplicación móvil multiplataforma para la gestión financiera personal y grupal, desarrollada con .NET MAUI. La aplicación proporciona capacidades integrales de seguimiento financiero incluyendo gestión de transacciones, compartición de gastos familiares, seguimiento de metas de ahorro y análisis financiero impulsado por IA.
</p>
<p>
  Este documento cubre la arquitectura general del sistema, dominios de negocio principales y componentes clave de la aplicación frontend.
</p>

<h2>🎯 Propósito y Alcance</h2>
<p>
  La aplicación móvil PiggyBank permite a los usuarios gestionar sus finanzas personales y familiares de manera intuitiva desde dispositivos Android, iOS, Windows y macOS, con integración completa al backend REST API.
</p>
<ul>
  <li>Interfaz de usuario nativa multiplataforma con .NET MAUI</li>
  <li>Gestión de transacciones con categorización avanzada</li>
  <li>Grupos familiares con gastos compartidos</li>
  <li>Metas financieras con seguimiento visual</li>
  <li>Asistente de IA para análisis financiero personalizado</li>
  <li>Autenticación segura con almacenamiento local</li>
</ul>

<h2>🧱 Arquitectura General</h2>
<p>Sistema basado en arquitectura por capas con separación clara entre presentación, lógica de negocio y acceso a datos:</p>
<ul>
  <li><strong>Capa de Presentación:</strong> XAML Pages + AppShell + Value Converters</li>
  <li><strong>Capa de Lógica de Negocio:</strong> ViewModels + Services + Data Models</li>
  <li><strong>Capa de Acceso a Datos:</strong> ApiService + Local Storage + Preferences</li>
</ul>

<p><strong>Servicios Externos:</strong> PiggyBank REST API, Almacenamiento Seguro Local</p>

<h3>🔗 Componentes Principales</h3>
<ul>
  <li><strong>Páginas Principales:</strong> PaginaHome, PaginaTransacciones, FamilyGroupsPage, FinancialGoalsPage</li>
  <li><strong>Páginas Modales:</strong> NuevaTransaccion, CreateFinancialGoalModalPage, UpdateGroupModalPage</li>
  <li><strong>Servicios:</strong> ApiService, SecureStorage, Preferences</li>
  <li><strong>Modelos:</strong> Entity.cs, Request.cs, Response.cs</li>
</ul>

<h2>🧩 Dominios de Negocio Principales</h2>

<h3>🔐 Dominio de Autenticación</h3>
<ul>
  <li><strong>Páginas:</strong> PaginaInicioDeSesion, PaginaRegistrarse, PaginaVerificarUsuario</li>
  <li><strong>Modelos de Request:</strong> ReqRegistrarUsuario, ReqIniciarSesion, ReqVerificarUsuario</li>
  <li><strong>Funcionalidades:</strong> Registro con verificación, inicio de sesión, recuperación de contraseña</li>
</ul>

<h3>💳 Dominio de Gestión de Transacciones</h3>
<ul>
  <li><strong>Páginas:</strong> PaginaTransacciones, NuevaTransaccion, ActualizarTransaccion</li>
  <li><strong>Modelos:</strong> Transaccion, TransaccionDTO, Categoria</li>
  <li><strong>Funcionalidades:</strong> CRUD de transacciones, categorización, filtros y búsqueda</li>
</ul>

<h3>👪 Dominio de Grupos Familiares</h3>
<ul>
  <li><strong>Páginas:</strong> FamilyGroupsPage, GroupDetailsPage, UpdateGroupModalPage</li>
  <li><strong>Modelos:</strong> GrupoFamiliar, MiembroGrupo, GastoCompartido</li>
  <li><strong>Funcionalidades:</strong> Creación de grupos, invitación de miembros, gastos compartidos</li>
</ul>

<h3>🎯 Dominio de Metas Financieras</h3>
<ul>
  <li><strong>Páginas:</strong> FinancialGoalsPage, FinancialGoalDetailsPage, CreateFinancialGoalModalPage</li>
  <li><strong>Modelos:</strong> MetaDTO, MetaTransaccionDTO</li>
  <li><strong>Funcionalidades:</strong> Creación de metas, seguimiento de progreso, asignación de transacciones</li>
</ul>

<h3>🤖 Dominio de Asistente IA</h3>
<ul>
  <li><strong>Páginas:</strong> PaginaAsistenteIA, AsistantHomePage</li>
  <li><strong>Modelos:</strong> AnalisisDTO, MensajeDTO, ContextoDTO</li>
  <li><strong>Funcionalidades:</strong> Chat interactivo, análisis financiero automatizado, recomendaciones</li>
</ul>

<h2>⚙️ Stack Tecnológico</h2>

<h3>🔧 Framework y Plataformas</h3>
<ul>
  <li><strong>Framework:</strong> .NET 8 MAUI</li>
  <li><strong>Plataformas Objetivo:</strong> Android, iOS, MacCatalyst, Windows</li>
  <li><strong>Lenguaje:</strong> C# 12</li>
  <li><strong>UI:</strong> XAML con estilos nativos por plataforma</li>
</ul>

<h3>📦 Dependencias Principales</h3>
<table>
  <thead>
    <tr><th>Componente</th><th>Tecnología</th><th>Propósito</th></tr>
  </thead>
  <tbody>
    <tr><td>HTTP Client</td><td>System.Net.Http</td><td>Comunicación con API</td></tr>
    <tr><td>JSON Processing</td><td>System.Text.Json</td><td>Serialización/deserialización</td></tr>
    <tr><td>UI Components</td><td>The49.Maui.BottomSheet</td><td>Controles UI mejorados</td></tr>
    <tr><td>Markdown</td><td>Markdig</td><td>Procesamiento de texto para respuestas IA</td></tr>
    <tr><td>Community Tools</td><td>CommunityToolkit.Maui</td><td>Utilidades adicionales MAUI</td></tr>
  </tbody>
</table>

<h3>📁 Estructura del Proyecto</h3>
<ul>
  <li><strong>Pages/:</strong> Páginas XAML y código subyacente</li>
  <li><strong>Models/:</strong> Entity.cs, Request.cs, Response.cs</li>
  <li><strong>Services/:</strong> ApiService.cs y servicios auxiliares</li>
  <li><strong>Resources/:</strong> Estilos, imágenes y recursos localizados</li>
  <li><strong>Platforms/:</strong> Código específico por plataforma</li>
</ul>

<h2>🔄 Flujo de Datos y Arquitectura del ApiService</h2>

<h3>📡 Componentes del ApiService</h3>
<ul>
  <li><strong>HttpClient:</strong> Cliente HTTP con BaseUrl configurada</li>
  <li><strong>Token Management:</strong> Gestión de tokens JWT (SetToken, ClearToken, UpdateAuthenticationHeader)</li>
  <li><strong>Exception Handling:</strong> Manejo centralizado de errores con ResBase.error</li>
  <li><strong>Request/Response Processing:</strong> Serialización automática de modelos</li>
</ul>

<h3>🔗 Métodos por Dominio</h3>
<ul>
  <li><strong>Autenticación:</strong> RegistrarUsuario, IniciarSesion, VerificarUsuario</li>
  <li><strong>Transacciones:</strong> IngresarTransaccion, ListarTransaccionesPorUsuario, ActualizarTransaccion</li>
  <li><strong>Grupos Familiares:</strong> CrearGrupoFamiliar, InvitarMiembroGrupo, RegistrarGastoCompartido</li>
  <li><strong>Metas Financieras:</strong> CrearMeta, ActualizarProgresoMeta, AsignarTransaccion</li>
  <li><strong>Asistente IA:</strong> ObtenerAnalisis, CrearAnalisis, InsertarMensaje</li>
</ul>

<h2>📊 Jerarquía de Modelos de Datos</h2>
<table>
  <thead>
    <tr><th>Categoría de Modelo</th><th>Clase Base</th><th>Ejemplos Clave</th></tr>
  </thead>
  <tbody>
    <tr><td>Modelos de Request</td><td>ReqBase</td><td>ReqRegistrarUsuario, ReqIngresarTransaccion, ReqCrearMeta</td></tr>
    <tr><td>Modelos de Response</td><td>ResBase</td><td>ResIniciarSesion, ResTransaccionesPorUsuario, ResListarMetas</td></tr>
    <tr><td>Modelos de Entidad</td><td>DTOs Varios</td><td>Usuario, TransaccionDTO, MetaDTO, GrupoDTO</td></tr>
    <tr><td>Objetos de Dominio</td><td>Entidades Core</td><td>Transaccion, GrupoFamiliar, MiembroGrupo</td></tr>
  </tbody>
</table>

<h2>🚨 Manejo de Errores y Almacenamiento</h2>
<table>
  <thead>
    <tr><th>Tipo de Error/Almacenamiento</th><th>Implementación</th><th>Propósito</th></tr>
  </thead>
  <tbody>
    <tr><td>Errores de API</td><td>ResBase.error</td><td>Manejo centralizado de errores del backend</td></tr>
    <tr><td>Excepciones HTTP</td><td>Try-catch en ApiService</td><td>Errores de conectividad y timeout</td></tr>
    <tr><td>Almacenamiento Seguro</td><td>SecureStorage</td><td>Credenciales y datos sensibles</td></tr>
    <tr><td>Preferencias</td><td>Preferences</td><td>Tokens de autenticación y configuración</td></tr>
  </tbody>
</table>

<h2>🎨 Características de la Interfaz de Usuario</h2>
<ul>
  <li><strong>Navegación:</strong> AppShell con pestañas principales y navegación modal</li>
  <li><strong>Estilos Nativos:</strong> Adaptación automática al sistema operativo</li>
  <li><strong>Responsive Design:</strong> Interfaces adaptables a diferentes tamaños de pantalla</li>
  <li><strong>Value Converters:</strong> Transformación de datos para presentación</li>
  <li><strong>Bottom Sheets:</strong> Modales deslizantes para mejor UX</li>
  <li><strong>Markdown Support:</strong> Renderizado de respuestas del asistente IA</li>
</ul>

<h2>👥 Usuarios Objetivo</h2>
<ul>
  <li>Usuarios móviles que requieren acceso offline limitado</li>
  <li>Familias que necesiten gestionar gastos compartidos desde dispositivos móviles</li>
  <li>Personas que prefieran interfaces táctiles nativas</li>
  <li>Usuarios que requieren sincronización entre múltiples dispositivos</li>
  <li>Usuarios de habla hispana en plataformas iOS y Android</li>
</ul>

<h2>🔄 Flujo de Datos Completo</h2>
<ol>
  <li>El usuario interactúa con la interfaz XAML</li>
  <li>El evento se procesa en el código subyacente de la página</li>
  <li>Se invoca el método correspondiente del ApiService</li>
  <li>ApiService serializa el request y lo envía al backend</li>
  <li>Se procesa la respuesta y se deserializa</li>
  <li>Los datos se actualizan en la interfaz de usuario</li>
  <li>Se maneja el almacenamiento local si es necesario</li>
</ol>

<h2>📌 Notas de Desarrollo</h2>
<ul>
  <li>La aplicación requiere conexión a internet para funcionalidad completa</li>
  <li>Los datos se almacenan localmente de forma temporal para mejorar la experiencia</li>
  <li>La autenticación JWT se maneja automáticamente en todas las requests</li>
</ul>

<h2>🔮 Resumen de Funcionalidades</h2>
<p>
  La aplicación PiggyBank MAUI proporciona gestión integral de finanzas personales a través de cinco áreas principales de funcionalidad:
</p>
<ul>
  <li><strong>Autenticación de Usuario:</strong> Gestión completa del ciclo de vida del usuario</li>
  <li><strong>Gestión de Transacciones:</strong> Seguimiento de ingresos y gastos con categorización</li>
  <li><strong>Grupos Familiares:</strong> Gestión de gastos compartidos con control de acceso basado en roles</li>
  <li><strong>Metas Financieras:</strong> Creación de metas de ahorro con seguimiento de progreso</li>
  <li><strong>Asistente IA:</strong> Sistema inteligente de análisis financiero y asesoramiento</li>
</ul>

<hr>
<p>Desarrollado por: <strong>Fabricio Alfaro Arce</strong>, Cristopher González, Nahum Mora y Orlando Mena | Curso: Diseño y Programación de Plataformas Móviles</p>

</body>
</html>
