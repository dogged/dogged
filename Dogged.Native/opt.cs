using System;

namespace Dogged.Native
{
    /// <summary>
    /// Global library options.  These are used to select which global
    /// option to seet or get and are used as the first argument when
    /// calling <see cref="git_libgit2_opts"/>.
    /// </summary>
    public enum git_libgit2_opt_t
    {
        /// <summary>
        /// Get the maximum mmap window size.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_MWINDOW_SIZE, size_t *):
        /// </para>
        /// </summary>
        GIT_OPT_GET_MWINDOW_SIZE,

        /// <summary>
        /// Set the maximum mmap window size.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_MWINDOW_SIZE, size_t);
        /// </para>
        /// </summary>
        GIT_OPT_SET_MWINDOW_SIZE,

        /// <summary>
        /// Get the maximum memory that will be mapped in total by the
        /// library.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_MWINDOW_MAPPED_LIMIT, size_t *):
        /// </para>
        /// </summary>
        GIT_OPT_GET_MWINDOW_MAPPED_LIMIT,

        /// <summary>
        /// Set the maximum amount of memory that can be mapped at any
        /// time by the library.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_MWINDOW_MAPPED_LIMIT, size_t):
        /// </para>
        /// </summary>
        GIT_OPT_SET_MWINDOW_MAPPED_LIMIT,

        /// <summary>
        /// Get the search path for a given level of config data.  "level"
        /// must be one of `GIT_CONFIG_LEVEL_SYSTEM`,
        /// `GIT_CONFIG_LEVEL_GLOBAL`, `GIT_CONFIG_LEVEL_XDG`, or
        /// `GIT_CONFIG_LEVEL_PROGRAMDATA`.  The search path is written
        /// to the given buffer.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_SEARCH_PATH, int level, git_buf *buf);
        /// </para>
        /// </summary>
        GIT_OPT_GET_SEARCH_PATH,

        /// <summary>
        /// Set the search path for a level of config data.  The search
        /// path applies to shared attributes and ignore files, too.
        /// Pass the level as one of `GIT_CONFIG_LEVEL_SYSTEM`,
        /// `GIT_CONFIG_LEVEL_GLOBAL`, `GIT_CONFIG_LEVEL_XDG`, or
        /// `GIT_CONFIG_LEVEL_PROGRAMDATA`.
        ///
        /// <para>
        /// Path directories must be delimited by GIT_PATH_LIST_SEPARATOR.
        /// Pass NULL to reset to the default (generally based on
        /// environment variables).  Use the magic path `$PATH` to include
        /// the old value of the path (if you want to prepend or append,
        /// for instance).
        /// </para>
        ///
        /// <para>
        /// opts(GIT_OPT_SET_SEARCH_PATH, int level, const char *path);
        /// </para>
        /// </summary>
        GIT_OPT_SET_SEARCH_PATH,

        /// <summary>
        /// Set the maximum data size for the given type of object to be
        /// considered eligible for caching in memory.  Setting to value to
        /// zero means that that type of object will not be cached.
        ///
        /// <para>
        /// Defaults to 0 for `GIT_OBJECT_BLOB` (meaning that blobs are not
        /// cached) and 4096 for `GIT_OBJECT_COMMIT`, `GIT_OBJECT_TREE`,
        /// and `GIT_OBJECT_TAG`.
        /// </para>
        ///
        /// <para>
        /// opts(GIT_OPT_SET_CACHE_OBJECT_LIMIT, git_object_t type, size_t size);
        /// </para>
        /// </summary>
        GIT_OPT_SET_CACHE_OBJECT_LIMIT,

        /// <summary>
        /// Set the maximum total data size that will be cached in memory
        /// across all repositories before libgit2 starts evicting objects
        /// from the cache.  This is a soft limit, in that the library might
        /// briefly exceed it, but will start aggressively evicting objects
        /// from cache when that happens.  The default cache size is 256MB.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_CACHE_MAX_SIZE, ssize_t max_storage_bytes);
        /// </para>
        /// </summary>
        GIT_OPT_SET_CACHE_MAX_SIZE,

        /// <summary>
        /// Enable or disable caching completely.
        ///
        /// <para>
        /// Because caches are repository-specific, disabling the cache
        /// cannot immediately clear all cached objects, but each cache
        /// will be cleared on the next attempt to update anything in it.
        /// </para>
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_CACHING, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_CACHING,

        /// <summary>
        /// Get the current bytes in cache and the maximum that would be
        /// allowed in the cache.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_CACHED_MEMORY, ssize_t *current, ssize_t *allowed);
        /// </para>
        /// </summary>
        GIT_OPT_GET_CACHED_MEMORY,

        /// <summary>
        /// Get the default template path.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_TEMPLATE_PATH, git_buf *out);
        /// </para>
        /// </summary>
        GIT_OPT_GET_TEMPLATE_PATH,

        /// <summary>
        /// Set the default template path.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_TEMPLATE_PATH, git_buf *out);
        /// </para>
        /// </summary>
        GIT_OPT_SET_TEMPLATE_PATH,

        /// <summary>
        /// Set the SSL certificate-authority locations.  The file is the
        /// location of a file containing several certificates concatenated
        /// together.  The path is the location of a directory holding
        /// serveral certificates, on per file.  Either parameter may be
        /// `NULL`, but not both.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_SSL_CERT_LOCATIONS, const char *file, const char *path);
        /// </para>
        /// </summary>
        GIT_OPT_SET_SSL_CERT_LOCATIONS,

        /// <summary>
        /// Set the value of the User-Agent header.  This value will be
        /// appended to "git/1.0", for compatibility with other git
        /// clients.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_USER_AGENT, const char *user_agent);
        /// </para>
        /// </summary>
        GIT_OPT_SET_USER_AGENT,

        /// <summary>
        ///  Enable strict input validation when creating new objects
        /// to ensure that all inputs to the new objects are valid.  For
        /// example, when this is enabled, the parent(s) and tree inputs
        /// will be validated when creating a new commit.  This defaults
        /// to enabled.
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_STRICT_OBJECT_CREATION, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_STRICT_OBJECT_CREATION,

        /// <summary>
        /// Validate the target of a symbolic ref when creating it.  For
        /// example, `foobar` is not a valid ref, therefore `foobar` is
        /// not a valid target for a symbolic ref by default, whereas
        /// `refs/heads/foobar` is.  Disabling this bypasses validation
        /// so that an arbitrary strings such as `foobar` can be used
        ///for a symbolic ref target.  This defaults to enabled.
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_STRICT_SYMBOLIC_REF_CREATION, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_STRICT_SYMBOLIC_REF_CREATION,

        /// <summary>
        /// Set the SSL ciphers use for HTTPS connections.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_SSL_CIPHERS, const char *ciphers);
        /// </para>
        /// </summary>
        GIT_OPT_SET_SSL_CIPHERS,

        /// <summary>
        /// Gets the value of the User-Agent header.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_USER_AGENT, git_buf *);
        /// </para>
        /// </summary>
        GIT_OPT_GET_USER_AGENT,

        /// <summary>
        /// Enable or disable the use of "offset deltas" when creating
        /// packfiles, and the negotiation of them when talking to a
        /// remote server.  Offset deltas store a delta base location as
        // an offset into the packfile from the current location, which
        /// provides a shorter encoding and thus smaller resultant
        /// packfiles.  This does not disable the ability to read
        /// offset deltas if a packfile containing one is encountered.
        /// This defaults to enabled.
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_OFS_DELTA, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_OFS_DELTA,

        /// <summary>
        /// Enable synchronized writes of files in the gitdir using `fsync`
        /// (or the platform equivalent) to ensure that new object data
        /// is written to permanent storage, not simply cached.  This
        /// defaults to disabled.
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_FSYNC_GITDIR, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_FSYNC_GITDIR,

        /// <summary>
        /// Get the share mode used when opening files on Windows.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_WINDOWS_SHAREMODE, unsigned long *value);
        /// </para>
        /// </summary>
        GIT_OPT_GET_WINDOWS_SHAREMODE,

        /// <summary>
        /// Set the share mode used when opening files on Windows.
        /// For more information, see the documentation for CreateFile.
        /// The default is: FILE_SHARE_READ | FILE_SHARE_WRITE.  This is
        /// ignored and unused on non-Windows platforms.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_WINDOWS_SHAREMODE, unsigned long value);
        /// </para>
        /// </summary>
        GIT_OPT_SET_WINDOWS_SHAREMODE,

        /// <summary>
        /// Enable strict verification of object hashsums when reading
        /// objects from disk. This may impact performance due to an
        /// additional checksum calculation on each object. This defaults
        /// to enabled.
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_STRICT_HASH_VERIFICATION, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_STRICT_HASH_VERIFICATION,

        /// <summary>
        ///  Set the memory allocator to a different memory allocator.
        /// This allocator will then be used to make all memory allocations
        /// for libgit2 operations.  If the given `allocator` is NULL, then
        /// the system default will be restored.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_ALLOCATOR, git_allocator *allocator);
        /// </para>
        /// </summary>
        GIT_OPT_SET_ALLOCATOR,

        /// <summary>
        /// Ensure that there are no unsaved changes in the index before
        /// beginning any operation that reloads the index from disk (eg,
        /// checkout).  If there are unsaved changes, the instruction will
        /// fail.  (Using the FORCE flag to checkout will still overwrite
        /// these changes.)
        ///
        /// <para>
        /// opts(GIT_OPT_ENABLE_UNSAVED_INDEX_SAFETY, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_ENABLE_UNSAVED_INDEX_SAFETY,

        /// <summary>
        /// Get the maximum number of objects libgit2 will allow in a pack
        /// file when downloading a pack file from a remote. This can be
        /// used to limit maximum memory usage when fetching from an untrusted
        /// remote.
        ///
        /// <para>
        /// opts(GIT_OPT_GET_PACK_MAX_OBJECTS, size_t *out);
        /// </para>
        /// </summary>
        GIT_OPT_GET_PACK_MAX_OBJECTS,

        /// <summary>
        /// Set the maximum number of objects libgit2 will allow in a pack
        /// file when downloading a pack file from a remote.
        ///
        /// <para>
        /// opts(GIT_OPT_SET_PACK_MAX_OBJECTS, size_t objects);
        /// </para>
        /// </summary>
        GIT_OPT_SET_PACK_MAX_OBJECTS,

        /// <summary>
        /// This will cause .keep file existence checks to be skipped when
        /// accessing packfiles, which can help performance with remote
        /// filesystems.
        ///
        /// <para>
        /// opts(GIT_OPT_DISABLE_PACK_KEEP_FILE_CHECKS, int enabled);
        /// </para>
        /// </summary>
        GIT_OPT_DISABLE_PACK_KEEP_FILE_CHECKS
    }
}
