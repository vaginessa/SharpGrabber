﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetTools.SharpGrabber.BlackWidow.Repository.Memory
{
    /// <summary>
    /// In-memory implementation of grabber repository.
    /// </summary>
    public class InMemoryRepository : GrabberRepositoryBase
    {
        private readonly Dictionary<string, ScriptInfo> _scripts = new();

        public override bool CanPut => true;

        public override Task<IGrabberScriptSource> FetchSourceAsync(IGrabberRepositoryScript script, CancellationToken cancellationToken)
        {
            var info = _scripts.GetOrDefault(script.Id);
            return Task.FromResult(info?.Source);
        }

        public override Task<IGrabberRepositoryFeed> GetFeedAsync(CancellationToken cancellationToken)
        {
            var feed = new GrabberRepositoryFeed(_scripts.Values.Select(i => i.Script));
            return Task.FromResult<IGrabberRepositoryFeed>(feed);
        }

        public override Task PutAsync(IGrabberRepositoryScript script, IGrabberScriptSource source, CancellationToken cancellationToken)
        {
            var info = new ScriptInfo(script, source);
            _scripts[script.Id] = info;
            return Task.CompletedTask;
        }

        private sealed class ScriptInfo
        {
            public ScriptInfo(IGrabberRepositoryScript script, IGrabberScriptSource source)
            {
                Script = script;
                Source = source;
            }

            public IGrabberRepositoryScript Script { get; }

            public IGrabberScriptSource Source { get; }
        }
    }
}
