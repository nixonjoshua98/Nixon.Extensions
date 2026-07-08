using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nixon.Extensions.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
    extension<T>(EntityTypeBuilder<T> builder) where T : class
    {
        public EntityTypeBuilder<T> DefineOneToOne<TChild>(Expression<Func<T, TChild?>> property,
            Expression<Func<TChild, object?>> foreignKey,
            bool autoInclude = true,
            DeleteBehavior deleteBehavior = DeleteBehavior.Cascade) where TChild : class
        {
            builder
                .HasOne(property)
                .WithOne()
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);

            builder
                .Navigation(property)
                .AutoInclude(autoInclude);

            return builder;
        }

        public EntityTypeBuilder<T> DefineOneToMany<TChild>(Expression<Func<TChild, object?>> foreignKey,
            DeleteBehavior deleteBehavior = DeleteBehavior.Cascade) where TChild : class
        {
            builder
                .HasMany<TChild>()
                .WithOne()
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);

            return builder;
        }

        public EntityTypeBuilder<T> DefineOneToMany<TChild>(Expression<Func<T, IEnumerable<TChild>?>> property,
            Expression<Func<TChild, object?>> foreignKey,
            bool autoInclude = true,
            DeleteBehavior deleteBehavior = DeleteBehavior.Cascade) where TChild : class
        {
            builder
                .HasMany(property)
                .WithOne()
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);

            builder
                .Navigation(property)
                .AutoInclude(autoInclude);

            return builder;
        }

        public EntityTypeBuilder<T> IncludeNavigation(Expression<Func<T, object?>> property,
            bool autoInclude)
        {
            builder
                .Navigation(property)
                .AutoInclude(autoInclude);

            return builder;
        }

        public EntityTypeBuilder<T> DefineOneToMany<TChild>(Expression<Func<T, IEnumerable<TChild>?>> property,
            Expression<Func<TChild, T?>> inverseProperty,
            Expression<Func<TChild, object?>> foreignKey,
            bool autoInclude = true,
            DeleteBehavior deleteBehavior = DeleteBehavior.Cascade) where TChild : class
        {
            builder
                .HasMany(property)
                .WithOne(inverseProperty)
                .HasForeignKey(foreignKey)
                .OnDelete(deleteBehavior);

            builder
                .Navigation(property)
                .AutoInclude(autoInclude);

            return builder;
        }
    }

    public static EntityTypeBuilder<TChild> DefineManyToOne<T, TChild>(
        this EntityTypeBuilder<TChild> builder,
        Expression<Func<TChild, T?>> referenceProperty,
        Expression<Func<TChild, object?>> foreignKey,
        bool autoInclude = true,
        DeleteBehavior deleteBehavior = DeleteBehavior.Cascade)
        where T : class
        where TChild : class
    {
        builder
            .HasOne(referenceProperty)
            .WithMany()
            .HasForeignKey(foreignKey)
            .OnDelete(deleteBehavior);

        builder
            .Navigation(referenceProperty)
            .AutoInclude(autoInclude);

        return builder;
    }

    public static EntityTypeBuilder<TChild> DefineOneToMany<T, TChild>(
        this EntityTypeBuilder<TChild> builder,
        Expression<Func<TChild, object?>> foreignKey,
        DeleteBehavior deleteBehavior = DeleteBehavior.Cascade)
        where T : class
        where TChild : class
    {
        builder
            .HasOne<T>()
            .WithMany()
            .HasForeignKey(foreignKey)
            .OnDelete(deleteBehavior);

        return builder;
    }

    public static EntityTypeBuilder<TChild> DefineOneToMany<T, TChild>(
        this EntityTypeBuilder<TChild> builder,
        Expression<Func<T, object?>> principalKey,
        Expression<Func<TChild, object?>> foreignKey,
        DeleteBehavior deleteBehavior = DeleteBehavior.Cascade)
        where T : class
        where TChild : class
    {
        builder
            .HasOne<T>()
            .WithMany()
            .HasForeignKey(foreignKey)
            .HasPrincipalKey(principalKey)
            .OnDelete(deleteBehavior);

        return builder;
    }
}