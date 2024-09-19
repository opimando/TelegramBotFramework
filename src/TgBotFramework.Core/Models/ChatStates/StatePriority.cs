#region Copyright

/*
 * File: StatePriority.cs
 * Author: denisosipenko
 * Created: 2023-11-14
 * Copyright © 2023 Denis Osipenko
 */

#endregion Copyright

namespace TgBotFramework.Core;

public enum StatePriority
{
    /// <summary>
    /// Если есть состояние, которому подходит сообщение и при этом CanBeIgnored состояние АКТИВНОЕ, то АКТИВНОЕ будет проигнорено в пользу нового подходящего состояния (даже если оно тоже canbeIgnored).
    /// </summary>
    CanBeIgnored,

    /// <summary>
    /// если у активного, то другие и искаться не будут. Если у вновь созданного то перекрывает активное. 
    /// </summary>
    Priority
}