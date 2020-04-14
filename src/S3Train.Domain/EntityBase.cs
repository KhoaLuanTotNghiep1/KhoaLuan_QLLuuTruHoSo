using System;

namespace S3Train.Domain
{
    /// <summary>
    /// Base class for database domain entity
    /// </summary>
    public abstract class EntityBase
    {
        public string Id { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public bool TrangThai { get; set; }
    }
}