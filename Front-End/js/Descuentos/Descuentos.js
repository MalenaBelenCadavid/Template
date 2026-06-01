// descuentos.js - lógica del dashboard de descuentos

const API_BASE = 'http://localhost:5211/api/v1';

let descuentos = [];
let nextId = 1;

// Inicializar
document.addEventListener('DOMContentLoaded', () => {
  const ahora = new Date();
  const fin = new Date(ahora);
  fin.setMonth(fin.getMonth() + 1);
  document.getElementById('fechaInicio').value = ahora.toISOString().slice(0, 16);
  document.getElementById('fechaFin').value = fin.toISOString().slice(0, 16);
  cargarDescuentos();
});

// Cargar descuentos desde el backend
async function cargarDescuentos() {
  try {
    const res = await fetch(`${API_BASE}/Descuento`);
    if (!res.ok) throw new Error();
    descuentos = await res.json();
    renderTabla();
  } catch {
    renderTabla();
  }
}

// Crear descuento
async function crearDescuento() {
  const tipo = document.getElementById('tipoDescuento').value;
  const valor = parseFloat(document.getElementById('valorDescuento').value);
  const descripcion = document.getElementById('descripcionDescuento').value.trim();
  const fechaInicio = document.getElementById('fechaInicio').value;
  const fechaFin = document.getElementById('fechaFin').value;

  if (!tipo) { mostrarToast('Seleccioná un tipo de descuento', 'error'); return; }
  if (!valor || valor <= 0 || valor > 100) { mostrarToast('El valor debe estar entre 1 y 100', 'error'); return; }
  if (!descripcion) { mostrarToast('Ingresá una descripción', 'error'); return; }
  if (!fechaInicio || !fechaFin) { mostrarToast('Ingresá las fechas', 'error'); return; }
  if (new Date(fechaFin) <= new Date(fechaInicio)) { mostrarToast('La fecha fin debe ser posterior a la fecha inicio', 'error'); return; }

  const body = {
    tipoDescuento: tipo,
    valor: valor,
    descripcion: descripcion,
    fechaInicio: new Date(fechaInicio).toISOString(),
    fechaFin: new Date(fechaFin).toISOString()
  };

  try {
    const res = await fetch(`${API_BASE}/Descuento`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    });
    if (!res.ok) throw new Error();
    const nuevo = await res.json();
    descuentos.push(nuevo);
    mostrarToast('✓ Descuento creado correctamente', 'success');
  } catch {
    descuentos.push({ idDescuento: nextId++, ...body, activo: true });
    mostrarToast('✓ Descuento creado (modo local)', 'success');
  }

  document.getElementById('tipoDescuento').value = '';
  document.getElementById('valorDescuento').value = '';
  document.getElementById('descripcionDescuento').value = '';
  renderTabla();
}

// Eliminar descuento
async function eliminarDescuento(id) {
  if (!confirm('¿Seguro que querés eliminar este descuento?')) return;
  try {
    await fetch(`${API_BASE}/Descuento/${id}`, { method: 'DELETE' });
  } catch { }
  descuentos = descuentos.filter(d => d.idDescuento !== id);
  renderTabla();
  mostrarToast('Descuento eliminado', 'error');
}

// Activar / Desactivar
async function toggleActivo(id) {
  const d = descuentos.find(d => d.idDescuento === id);
  if (!d) return;
  d.activo = !d.activo;
  try {
    await fetch(`${API_BASE}/Descuento/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(d)
    });
  } catch { }
  renderTabla();
  mostrarToast(d.activo ? '✓ Descuento activado' : 'Descuento desactivado', d.activo ? 'success' : 'error');
}

// Filtrar tabla
function filtrar() { renderTabla(); }

// Renderizar tabla
function renderTabla() {
  const filtro = document.getElementById('filtroTipo').value;
  const lista = filtro ? descuentos.filter(d => d.tipoDescuento === filtro) : descuentos;
  const tbody = document.getElementById('tablaDescuentos');

  if (lista.length === 0) {
    tbody.innerHTML = `<tr><td colspan="7" class="empty">No hay descuentos${filtro ? ' de tipo ' + filtro : ''} registrados.</td></tr>`;
    return;
  }

  tbody.innerHTML = lista.map(d => {
    const tipo = d.tipoDescuento?.toLowerCase() || 'general';
    const tipoBadge = tipo === 'reserva' ? 'badge-reserva' : tipo === 'inscripcion' ? 'badge-inscripcion' : 'badge-general';
    const fi = new Date(d.fechaInicio).toLocaleDateString('es-AR');
    const ff = new Date(d.fechaFin).toLocaleDateString('es-AR');

    return `
      <tr>
        <td style="color:var(--texto-suave)">#${d.idDescuento}</td>
        <td><span class="badge ${tipoBadge}">${d.tipoDescuento}</span></td>
        <td><span class="valor-badge">${d.valor}%</span></td>
        <td style="max-width:200px; color:var(--texto-suave); font-size:0.82rem">${d.descripcion}</td>
        <td style="font-size:0.8rem; color:var(--texto-suave)">${fi} → ${ff}</td>
        <td><span class="badge ${d.activo ? 'badge-activo' : 'badge-inactivo'}">${d.activo ? 'Activo' : 'Inactivo'}</span></td>
        <td>
          <div class="btns-row">
            <button class="action-btn" onclick="toggleActivo(${d.idDescuento})">${d.activo ? 'Desactivar' : 'Activar'}</button>
            <button class="action-btn eliminar" onclick="eliminarDescuento(${d.idDescuento})">Eliminar</button>
          </div>
        </td>
      </tr>
    `;
  }).join('');
}

// Simulador de precio para socios
async function simular() {
  const precio = parseFloat(document.getElementById('simPrecio').value);
  const tipo = document.getElementById('simTipo').value;
  const resultado = document.getElementById('resultadoSim');

  if (!precio || precio <= 0) { mostrarToast('Ingresá un precio válido', 'error'); return; }

  let descuento = null;

  try {
    const res = await fetch(`${API_BASE}/Descuento/vigente/${tipo}`);
    if (res.ok) descuento = await res.json();
  } catch {
    const ahora = new Date();
    descuento = descuentos.find(d =>
      d.activo &&
      (d.tipoDescuento === tipo || d.tipoDescuento === 'General') &&
      new Date(d.fechaInicio) <= ahora &&
      new Date(d.fechaFin) >= ahora
    );
  }

  resultado.classList.add('visible');

  if (!descuento) {
    resultado.innerHTML = `<div class="sin-descuento">⚠ No hay descuento activo para este tipo. Precio: <strong>$${precio.toLocaleString('es-AR')}</strong></div>`;
    return;
  }

  const ahorro = precio * (descuento.valor / 100);
  const precioFinal = precio - ahorro;

  resultado.innerHTML = `
    <div style="margin-bottom:0.3rem">
      <span class="badge badge-${descuento.tipoDescuento.toLowerCase()}" style="margin-right:0.5rem">${descuento.tipoDescuento}</span>
      <span style="font-size:0.82rem; color:var(--texto-suave)">${descuento.descripcion}</span>
    </div>
    <div class="precio-original">Precio original: $${precio.toLocaleString('es-AR')}</div>
    <div class="precio-final">$${precioFinal.toLocaleString('es-AR')}</div>
    <div class="ahorro">✓ Ahorrás $${ahorro.toLocaleString('es-AR')} (${descuento.valor}% de descuento para socios)</div>
  `;
}

// Toast
function mostrarToast(mensaje, tipo) {
  const toast = document.getElementById('toast');
  toast.textContent = mensaje;
  toast.className = `toast ${tipo} show`;
  setTimeout(() => toast.classList.remove('show'), 3000);
}
