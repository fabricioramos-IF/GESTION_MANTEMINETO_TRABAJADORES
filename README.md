# Gestión de Mantenimiento de Trabajadores

**Prueba Técnica** — Analista Programador .NET | MYPER Software  
**Autor:** Fabricio Ramos | **Fecha:** Febrero 2026

---

## Stack Técnico

| Componente | Detalle |
|---|---|
| Backend | .NET 8, ASP.NET Core MVC, C# 12 |
| ORM | Entity Framework Core 8.0.11 |
| Base de datos | SQL Server Express — BD: `TrabajadoresPrueba` |
| Frontend | Bootstrap 5, Bootstrap Icons 1.11.3, jQuery 3.x, Fetch API |
| Testing | xUnit 2.6.6, Moq 4.20.72 |

---

## Arquitectura

Patrón **MVC + Capa de Servicios** con inyección de dependencias:

```
Views (Razor + Bootstrap + JS)
    ↓
Controllers (TrabajadorController — orquesta y valida)
    ↓
Services (ITrabajadorService / IFileUploadService — lógica de negocio)
    ↓
Data (ApplicationDbContext + Stored Procedures)
    ↓
SQL Server (Tabla Trabajador + 4 SPs)
```

**Patrones aplicados:** MVC, Inyección de Dependencias (Scoped), Interfaces, ViewModel, Soft Delete, Validador custom (`EdadMinimaAttribute`), Anti-Forgery Token (CSRF), Procedimientos Almacenados.

---

## Instalación

### 1. Clonar y configurar BD

```bash
git clone https://github.com/fabricioramos-IF/GESTION_MANTEMINETO_TRABAJADORES.git
cd GESTION_MANTEMINETO_TRABAJADORES
```

Ejecutar `DB_TRABJADORESPRUEBA.sql` en sql server

### 2. Configurar conexión

En `Trabajadores.Web/appsettings.json` ajustar el servidor:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TrabajadoresPrueba;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Ejecutar

```bash
cd Trabajadores.Web
dotnet restore
dotnet run
```


### 4. Tests

```bash
dotnet test
```

**Requisitos:** .NET SDK 8.0, SQL Server Express, navegador moderno.

---

## Base de Datos

**Tabla `Trabajador`:** IdTrabajador (PK, identity), Nombres, Apellidos, TipoDocumento, NumeroDocumento, Sexo (CHECK M/F), FechaNacimiento, Foto, Direccion, FechaRegistro (default GETDATE), Activo (default 1).

**Constraints:** `UNIQUE(TipoDocumento, NumeroDocumento)` — evita documentos duplicados.

**Procedimientos Almacenados:**

| SP | Función |
|---|---|
| `sp_listar_trabajadores(@Sexo)` | Lista activos con filtro opcional por sexo |
| `sp_insertar_trabajador` | Inserta nuevo registro |
| `sp_actualizar_trabajador` | Actualiza datos de un activo |
| `sp_eliminar_trabajador` | Soft delete: `Activo = 0` |

---

## Funcionalidades

| Funcionalidad | Descripción |
|---|---|
| **Listado** | Tabla con foto/avatar, datos completos, badges de color por sexo. Vista dual: tabla (desktop) / cards (móvil, < 992px) |
| **Registro** | Modal con formulario completo. Campos: Nombres, Apellidos, Tipo Doc (DNI/RUC/Pasaporte/CE), Nro Doc, Sexo, Fecha Nacimiento, Foto, Dirección. Envío asíncrono vía Fetch API |
| **Edición** | Mismo modal, precarga datos vía AJAX. Si se sube nueva foto, elimina la anterior del servidor |
| **Eliminación** | Modal de confirmación: *"¿Está seguro de eliminar el registro?"*. Soft delete + limpieza de foto |
| **Filtros** | Chips: Todos / Masculino (azul) / Femenino (naranja). Ejecutados via SP con parámetro `@Sexo` |
| **Notificaciones** | Toasts Bootstrap: verde (éxito), rojo (error) |

---

## Validaciones (3 capas)

| Capa | Implementación |
|---|---|
| **Cliente** | HTML5 `required`/`pattern`, JS `oninput` (solo números en documento), `calcularEdad()` para edad mínima, `accept=".jpg,.jpeg,.png"` |
| **Servidor** | DataAnnotations: `[Required]`, `[StringLength]`, `[RegularExpression]`, `[EdadMinima(18)]` custom. ModelState en controladores |
| **BD** | `CHECK (Sexo IN ('M','F'))`, `UNIQUE(TipoDocumento, NumeroDocumento)`, `NOT NULL` |

**`EdadMinimaAttribute`:** Valida edad >= 18 años, rechaza fechas futuras, anteriores a 1900 y edades > 120.

**`FileUploadService`:** Solo `.jpg/.jpeg/.png`, máximo 2 MB, nombres con GUID, almacena en `wwwroot/images/trabajadores/`.

---

## Decisiones Técnicas Clave

1. **Stored Procedures para todo el CRUD** — Recomendado por el requerimiento. Parametrización nativa, optimización en BD, centralización de lógica de datos.
2. **Soft Delete** — `Activo = 0` en lugar de DELETE físico. Permite recuperación, auditoría y preserva integridad referencial.
3. **Model vs ViewModel** — `Trabajador.cs` (entidad BD) separado de `TrabajadorViewModel.cs` (tiene `IFormFile` y validaciones de UI). No contamina el dominio.
4. **Interfaces + DI Scoped** — `ITrabajadorService`, `IFileUploadService` inyectados. Facilita testing con Moq y sigue SOLID.
5. **Fetch API en modales** — CRUD sin recargar página. Respuestas JSON `{ exito, mensaje }`. Protección CSRF con AntiForgeryToken.
6. **Vista dual desktop/mobile** — Tabla > 992px, cards <= 991px. Toggle por CSS media queries, misma data.

---

## Pruebas Unitarias (6 tests — xUnit + Moq)

| Test | Qué verifica |
|---|---|
| `Index_RetornaListaDeTrabajadores` | Controller Index retorna ViewResult con lista |
| `Obtener_IdNoExistente_RetornaNotFound` | ID inexistente retorna 404 |
| `Trabajador_DatosCompletos_ValidacionExitosa` | Modelo válido pasa DataAnnotations |
| `Trabajador_SexoInvalido_ValidacionFalla` | Sexo "X" falla validación regex |
| `Validar_EdadMayor18_RetornaExito` | 25 años pasa `EdadMinimaAttribute` |
| `Validar_EdadMenor18_RetornaError` | 17 años falla validación |

---

## Estructura del Proyecto

```
├── DB_TRABJADORESPRUEBA.sql        # Script BD + tabla + 4 SPs
├── Trabajadores.Web/
│   ├── Program.cs                  # Configuración DI y pipeline
│   ├── Controllers/
│   │   └── TrabajadorController.cs # CRUD (Index, Obtener, Crear, Editar, Eliminar)
│   ├── Models/Trabajador.cs        # Entidad de dominio
│   ├── ViewModels/TrabajadorViewModel.cs # Modelo de vista con IFormFile
│   ├── Services/
│   │   ├── ITrabajadorService.cs   # Interfaz CRUD
│   │   ├── TrabajadorService.cs    # Implementación con SPs
│   │   ├── IFileUploadService.cs   # Interfaz upload
│   │   └── FileUploadService.cs    # Upload, validación, borrado de fotos
│   ├── Data/ApplicationDbContext.cs # DbContext con Fluent API
│   ├── Validators/EdadMinimaAttribute.cs # Validador custom 18+
│   ├── Tests/                      # 3 archivos de test (xUnit + Moq)
│   └── Views/Trabajador/Index.cshtml # Vista CRUD: tabla + modales + JS
```

---

## Mejoras Futuras

- Paginación servidor, búsqueda por texto, exportación PDF/Excel
- Autenticación con Identity, auditoría con timestamps
- Logging con Serilog, caché en memoria
- Docker, CI/CD con GitHub Actions

---

© 2026 Fabricio Ramos — Prueba técnica  