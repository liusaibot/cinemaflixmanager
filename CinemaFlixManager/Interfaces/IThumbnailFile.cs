﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaFlixManager.Interfaces
{
    public interface IThumbnailFile
    {
        string ContentType { get; }

        string ContentDisposition { get; }

        IHeaderDictionary Headers { get; }

        long Length { get; }

        string Name { get; }

        string FileName { get; }

        Stream OpenReadStream();

        void CopyTo(Stream target);

        //Task CopyToAsync(Stream target, CancellationToken cancellationToken = null);
    }
}
