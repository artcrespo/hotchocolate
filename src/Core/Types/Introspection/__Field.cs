using HotChocolate.Configuration;

namespace HotChocolate.Types.Introspection
{
    internal sealed class __Field
        : ObjectType<Field>
    {
        protected override void Configure(IObjectTypeDescriptor<Field> descriptor)
        {
            descriptor.Name("__Field");

            descriptor.Description(
                "Object and Interface types are described by a list of Fields, each of " +
                "which has a name, potentially a list of arguments, and a return type.");

            descriptor.BindFields(BindingBehavior.Explicit);

            descriptor.Field(t => t.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(t => t.Description);

            descriptor.Field(t => t.Arguments)
                .Name("args")
                .Type<NonNullType<ListType<NonNullType<__InputValue>>>>()
                .Resolver(c => c.Parent<Field>().Arguments.Values);

            descriptor.Field(t => t.Type)
                .Type<NonNullType<__Type>>();

            descriptor.Field(t => t.IsDeprecated)
                .Type<NonNullType<BooleanType>>();

            descriptor.Field(t => t.DeprecationReason);
        }
    }
}
