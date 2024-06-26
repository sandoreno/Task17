﻿namespace backend.Models
{
    /// <summary>
    /// Тип корабля
    /// </summary>
    public class ShipClass
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Тип корабля
        /// </summary>
        public string ShipType { get; set; } = null!;
            
        /// <summary>
        /// Отношение к кораблю
        /// </summary>
        public List<Ship> Ship { get; set; } = null!;
        public List<ShipClassIntegerClass> ShipClassIntegerClasses { get; set; } = new List<ShipClassIntegerClass>();
    }
}