namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Img
{
    public Guid ImagesId { get; set; }

    public Guid AnswerId { get; set; }

    public byte[] Img1 { get; set; } = null!;

    public virtual Answer Answer { get; set; } = null!;
}
