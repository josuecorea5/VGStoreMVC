namespace VGStore.Models.ViewModels
{
    public class DetallesVM
    {
        public DetallesVM()
        {
            Producto = new Productos();
        }
        public Productos Producto { get; set;}
        public Categories Categories { get; set;}
        public Consoles Consoles { get; set;}
        public bool EnCarrito { get; set; }
    }
}
