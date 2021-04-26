// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace BookApp.Books.Domain.SupportTypes
{
    public interface ISoftDelete
    {
        bool SoftDeleted { get; }
        void AlterSoftDelete(bool softDeleted);
    }
}