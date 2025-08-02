namespace PropVivo.Application.Dto.AzureStorage
{
    public class BlobDto
    {
        public Stream? Content { get; set; }
        public string? ContentType { get; set; }
        public string? Name { get; set; }

        public string? Path { get; set; }
        public string? Uri { get; set; }
    }

    public class BlobResponseDto
    {
        public BlobResponseDto()
        {
            Blob = new BlobDto();
        }

        public BlobDto Blob { get; set; }
        public bool Error { get; set; }
        public string? Status { get; set; }
    }
}