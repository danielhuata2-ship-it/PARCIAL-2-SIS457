using CadParcial2Jdhf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnParcial2Jdhf
{
    public class CategoriaProgramaCln
    {
        public static int insertar(CategoriaPrograma categoriaPrograma)
        {
            using (var context = new Parcial2JdhfEntities1())
            {
                context.CategoriaPrograma.Add(categoriaPrograma);
                context.SaveChanges();
                return categoriaPrograma.id;
            }
        }

        public static int actualizar(CategoriaPrograma categoriaPrograma)
        {
            using (var context = new Parcial2JdhfEntities1())
            {
                var existe = context.CategoriaPrograma.Find(categoriaPrograma.id);
                existe.nombre = categoriaPrograma.nombre;
                existe.estado = categoriaPrograma.estado;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id)
        {
            using (var context = new Parcial2JdhfEntities1())
            {
                var existe = context.CategoriaPrograma.Find(id);
                existe.estado = -1;
                return context.SaveChanges();
            }
        }

        public static CategoriaPrograma obtenerUno(int id)
        {
            using (var context = new Parcial2JdhfEntities1())
            {
                return context.CategoriaPrograma.Find(id);
            }
        }

        public static List<CategoriaPrograma> listar()
        {
            using (var context = new Parcial2JdhfEntities1())
            {
                return context.CategoriaPrograma
                    .Where(x => x.estado != -1)
                    .ToList();
            }
        }

        public static List<paCategoriaProgramaListar_Result> listarPa(string parametro)
        {
            using (var context = new Parcial2JdhfEntities1())
            {
                return context.paCategoriaProgramaListar(parametro).ToList();
            }
        }
    }
}