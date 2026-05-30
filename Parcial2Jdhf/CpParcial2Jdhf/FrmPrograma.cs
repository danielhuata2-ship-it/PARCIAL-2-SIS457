using CadParcial2Jdhf;
using ClnParcial2Jdhf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Jdhf
{
    public partial class FrmPrograma : Form
    {

        private bool esNuevo = false;
        public FrmPrograma()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var lista = ProgramaCln.listarPa(txtParametro.Text.Trim());
            dgvLista.AutoGenerateColumns = true;
            dgvLista.DataSource = lista;
            dgvLista.Refresh();
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["idCanal"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["descripcion"].HeaderText = "Descripción";
            dgvLista.Columns["nombreCanal"].HeaderText = "Nombre Canal";
            dgvLista.Columns["duracion"].HeaderText = "Duración";
            dgvLista.Columns["productor"].HeaderText = "Productor";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha de Estreno";

            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];
            btnEditar.Enabled = lista.Count > 0;
            btnBorrar.Enabled = lista.Count > 0;
        }

        private void cargarCanal()
        {
            var lista = CanalCln.listar();
            cbxCanal.DataSource = lista;
            cbxCanal.ValueMember = "id";
            cbxCanal.DisplayMember = "nombre";
            cbxCanal.SelectedIndex = -1;
        }

        private void FrmPrograma_Load(object sender, EventArgs e)
        {
            Size = new Size(932, 405);
            listar();
            cargarCanal();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            pnlAcciones.Enabled = false;
            Size = new Size(932, 572); 
            txtTitulo.Focus();
        }

        private void reiniciarErrores()
        {
            erpCanal.Clear();
            erpTitulo.Clear();
            erpDescripcion.Clear();
            erpDuracion.Clear();
            erpProductor.Clear();
            erpFechaCreacion.Clear();
        }
        private void limpiar()
        {
            cbxCanal.SelectedIndex = -1;
            txtTitulo.Clear();
            txtDescripcion.Clear();
            nudDuracion.Value = 0;
            txtProductor.Clear();
            dtpFechaCreacion.Value = DateTime.Now;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            pnlAcciones.Enabled = false;
            Size = new Size(932, 572);

            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            var programa = ProgramaCln.obtenerUno(id);
            cbxCanal.SelectedValue = programa.idCanal;
            txtTitulo.Text = programa.titulo;
            txtDescripcion.Text = programa.descripcion;
            nudDuracion.Value = programa.duracion;
            txtProductor.Text = programa.productor;
            dtpFechaCreacion.Value = programa.fechaEstreno;

            txtTitulo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlAcciones.Enabled = true;
            limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }
        private bool validar()
        {
            bool esValido = true;
            reiniciarErrores();

            if (string.IsNullOrEmpty(cbxCanal.Text))
            {
                erpCanal.SetError(cbxCanal, "El Canal es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                erpTitulo.SetError(txtTitulo, "El Título es obligatorio");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                erpDescripcion.SetError(txtDescripcion, "La Descripción es obligatoria");
                esValido = false;
            }
            if (string.IsNullOrEmpty(nudDuracion.Value.ToString()))
            {
                erpDuracion.SetError(nudDuracion, "La Duración es obligatoria");
                esValido = false;
            }
            if (string.IsNullOrEmpty(txtProductor.Text))
            {
                erpProductor.SetError(txtProductor, "El Productor es obligatorio");
                esValido = false;
            }
            if (dtpFechaCreacion.Value >= DateTime.Now)
            {
                erpFechaCreacion.SetError(dtpFechaCreacion, "La Fecha de Creacion debe ser válida (no futura)");
                esValido = false;
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var programa = new Programa();
                programa.titulo = txtTitulo.Text.Trim();
                programa.descripcion = txtDescripcion.Text.Trim();
                programa.productor = txtProductor.Text.Trim();
                programa.idCanal = (int)cbxCanal.SelectedValue;
                programa.duracion = (int)nudDuracion.Value;
                programa.fechaEstreno = dtpFechaCreacion.Value;

                programa.usuarioRegistro = "admin";

                if (esNuevo)
                {
                    programa.fechaRegistro = DateTime.Now;
                    programa.estado = 1;

                    ProgramaCln.insertar(programa);

                    MessageBox.Show("Programa agregado correctamente",
                        "::: Mensaje - Programa :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    programa.id = (int)dgvLista.CurrentRow.Cells["id"].Value;
                    programa.fechaRegistro = DateTime.Now;

                    ProgramaCln.actualizar(programa);

                    MessageBox.Show("Programa actualizado correctamente",
                        "::: Mensaje - Programa :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                listar();
                btnCancelar.PerformClick(); 

                MessageBox.Show("Programa guardado correctamente",
                    "::: Mensaje - Programa :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            int id = (int)dgvLista.CurrentRow.Cells["id"].Value;
            string titulo = dgvLista.CurrentRow.Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar el programa {titulo}?",
                "::: Mensaje - Programa :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                ProgramaCln.eliminar(id);
                listar();
                MessageBox.Show("Programa dado de baja correctamente", "::: Mensaje - Programa :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
