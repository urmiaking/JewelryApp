﻿namespace JewelryApp.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScopedServiceAttribute<TService> : Attribute
{
}