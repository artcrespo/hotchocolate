using System;
using System.Linq.Expressions;
using HotChocolate.Configuration;
using HotChocolate.Language;

namespace HotChocolate.Types
{
    public interface IObjectTypeDescriptor
        : IFluent
    {
        IObjectTypeDescriptor SyntaxNode(ObjectTypeDefinitionNode syntaxNode);

        IObjectTypeDescriptor Name(string name);

        IObjectTypeDescriptor Description(string description);

        IObjectTypeDescriptor Interface<T>()
            where T : InterfaceType;

        IObjectTypeDescriptor Interface(NamedTypeNode type);

        IObjectTypeDescriptor IsOfType(IsOfType isOfType);

        IFieldDescriptor Field(string name);
    }

    public interface IObjectTypeDescriptor<T>
        : IObjectTypeDescriptor
    {
        new IObjectTypeDescriptor<T> Name(string name);

        new IObjectTypeDescriptor<T> Description(string description);

        IObjectTypeDescriptor<T> BindFields(BindingBehavior bindingBehavior);

        new IObjectTypeDescriptor<T> Interface<TInterface>()
            where TInterface : InterfaceType;

        new IObjectTypeDescriptor<T> IsOfType(IsOfType isOfType);

        IFieldDescriptor Field<TValue>(Expression<Func<T, TValue>> property);
    }
}
