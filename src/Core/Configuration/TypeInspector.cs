﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using HotChocolate.Configuration;
using HotChocolate.Types;

namespace HotChocolate.Configuration
{
    internal class TypeInspector
        : ITypeInfoFactory
    {
        private static readonly ITypeInfoFactory[] _factories = new ITypeInfoFactory[]
        {
            new NamedTypeInfoFactory(),
            new DotNetTypeInfoFactory()
        };

        private ImmutableDictionary<Type, TypeInfo> _typeInfoCache
            = ImmutableDictionary<Type, TypeInfo>.Empty;

        public bool TryCreate(Type type, out TypeInfo typeInfo)
        {
            if (!_typeInfoCache.TryGetValue(type, out typeInfo))
            {
                if (!TryCreateInternal(type, out typeInfo))
                {
                    typeInfo = default;
                    return false;
                }

                lock (_typeInfoCache)
                {
                    _typeInfoCache = _typeInfoCache.SetItem(type, typeInfo);
                }
            }
            return true;
        }

        private bool TryCreateInternal(Type type, out TypeInfo typeInfo)
        {
            foreach (ITypeInfoFactory factory in _factories)
            {
                if (factory.TryCreate(type, out typeInfo))
                {
                    return true;
                }
            }

            typeInfo = default;
            return false;
        }
    }
}
