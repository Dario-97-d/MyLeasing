namespace MyLeasing.Web.Models
{
    public class ObjectNotFoundViewModel
    {
        public string ObjectTypeName { get; set; }

        public ObjectNotFoundViewModel(string objectTypeName)
        {
            ObjectTypeName = objectTypeName;
        }
    }
}
