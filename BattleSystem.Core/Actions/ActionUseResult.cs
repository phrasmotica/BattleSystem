﻿using System.Collections.Generic;

namespace BattleSystem.Core.Actions
{
    /// <summary>
    /// Represents the result of an action being used.
    /// </summary>
    public class ActionUseResult<TSource>
    {
        /// <summary>
        /// Gets or sets whether the action use succeeded.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the results of the action being used successfully.
        /// </summary>
        public IEnumerable<IActionResult<TSource>> Results { get; set; }

        /// <summary>
        /// Gets or sets the tags for the actions result.
        /// </summary>
        public HashSet<string> Tags { get; set; }
    }
}
