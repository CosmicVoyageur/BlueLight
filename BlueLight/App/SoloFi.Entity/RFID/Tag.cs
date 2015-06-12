namespace SoloFi.Entity.RFID
{
    public class Tag
    {

        public string TagNumber { get; set; }

        public override bool Equals(object obj)
        {
            var tag = obj as Tag;
            if (tag == null) return false;

            return tag.TagNumber == this.TagNumber;
        }
    }
}
