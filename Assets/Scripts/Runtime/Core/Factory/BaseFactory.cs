﻿namespace Core
{
    public abstract class BaseFactory<T>
    {
        public abstract T Create();
    }
}