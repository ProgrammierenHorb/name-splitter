namespace NameSplitter.DTOs
{
    /// <summary>
    /// Dto for new titles, including the regex expression and the associated title
    /// </summary>
    public class TitleDto
    {
        public string Regex { get; set; }
        public string Title { get; set; }
    }
}