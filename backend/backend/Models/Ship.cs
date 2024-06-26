﻿namespace backend.Models
{
    /// <summary>
    /// Параметры корабля
    /// </summary>
    public class Ship
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Имя корабля
        /// </summary>
        public string ShipName { get; set; } = null!;

        /// <summary>
        /// Отношение к классу корабля
        /// </summary>
        public ShipClass ShipClass { get; set; } = null!;

        /// <summary>
        /// Скорость
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Gets or sets the ship class identifier.
        /// </summary>
        /// <value>
        /// The ship class identifier.
        /// </value>
        public Guid ShipClassId { get; set; }
    }
}
