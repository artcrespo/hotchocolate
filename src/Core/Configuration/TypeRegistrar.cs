
using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Types;

namespace HotChocolate.Configuration
{
    /// <summary>
    /// Registers types and their depending types.
    /// </summary>
    internal class TypeRegistrar
    {
        private readonly Queue<INamedType> _queue;
        private readonly HashSet<string> _registered = new HashSet<string>();
        private readonly List<SchemaError> _errors = new List<SchemaError>();

        public TypeRegistrar(IEnumerable<INamedType> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            _queue = new Queue<INamedType>(types);
        }

        public IReadOnlyCollection<SchemaError> Errors => _errors;

        public void RegisterTypes(ISchemaContext context)
        {
            RegisterAllTypes(context);
            RegisterTypeDependencies(context);
        }

        private void RegisterAllTypes(ISchemaContext context)
        {
            foreach (INamedType type in _queue)
            {
                context.Types.RegisterType(type);
            }
        }

        private void RegisterTypeDependencies(
            ISchemaContext context)
        {
            // register types until there are no new registrations of types.
            while (_queue.Any())
            {
                // process current batch of types.
                ProcessBatch(context);

                // check if there are new types that have to be processed.
                EnqueueUnprocessedTypes(context.Types);
            }
        }

        private void ProcessBatch(ISchemaContext context)
        {
            while (_queue.Any())
            {
                INamedType type = _queue.Dequeue();
                if (!_registered.Contains(type.Name))
                {
                    _registered.Add(type.Name);
                    context.Types.RegisterType(type);
                    type = context.Types.GetType<INamedType>(type.Name);

                    if (type is INeedsInitialization initializer)
                    {
                        initializer.RegisterDependencies(
                            context, e => _errors.Add(e));
                    }
                }
            }
        }

        private void EnqueueUnprocessedTypes(ITypeRegistry types)
        {
            foreach (INamedType type in types.GetTypes())
            {
                if (!_registered.Contains(type.Name))
                {
                    _queue.Enqueue(type);
                }
            }
        }
    }
}
