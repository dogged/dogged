using System;
using System.Collections.Generic;
using Dogged.Native;
using Dogged.Native.Services;

namespace Dogged
{
    /// <summary>
    /// Representation of a file entry in the index.
    /// </summary>
    public class IndexEntry
    {
        /// <summary>
        /// Create an index entry for a file with the given stage
        /// information.
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="mode">The filemode for the file</param>
        /// <param name="id">The object id for the file</param>
        public IndexEntry(string path, FileMode mode, ObjectId id) :
            this(path, mode, id, IndexEntryStage.Main) { }

        /// <summary>
        /// Create an index entry for a file with the given stage
        /// and cache information.
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="mode">The filemode for the file</param>
        /// <param name="id">The object id for the file</param>
        /// <param name="stage">The stage level to add</param>
        /// <param name="flags">Flags for the index entry</param>
        /// <param name="changeTime">The last changed time for the item</param>
        /// <param name="modificationTime">The last modified time for the item</param>
        /// <param name="device">The device the working tree file is on</param>
        /// <param name="inode">The inode for the working tree file</param>
        /// <param name="userId">The uid that owns the working tree file</param>
        /// <param name="groupId">The gid that owns the working tree file</param>
        /// <param name="fileSize">The file size</param>
        public IndexEntry(
            string path,
            FileMode mode,
            ObjectId id,
            IndexEntryStage stage = IndexEntryStage.Main,
            IndexEntryFlags flags = IndexEntryFlags.None,
            IndexEntryTime changeTime = null,
            IndexEntryTime modificationTime = null,
            long device = 0,
            long inode = 0,
            long userId = 0,
            long groupId = 0,
            long fileSize = 0)
        {
            Ensure.ArgumentNotNull(path, "path");
            Ensure.ArgumentNotNull(mode, "mode");
            Ensure.ArgumentNotNull(id, "id");
            Ensure.CastToUInt(device, "device");
            Ensure.CastToUInt(inode, "inode");
            Ensure.CastToUInt(inode, "userId");
            Ensure.CastToUInt(inode, "groupId");
            Ensure.CastToUInt(inode, "fileSize");

            Path = path;
            Mode = mode;
            Id = id;
            Stage = stage;
            Flags = new IndexEntryFlags();
            ChangeTime = changeTime ?? IndexEntryTime.Epoch;
            ModificationTime = modificationTime ?? IndexEntryTime.Epoch;
            Device = device;
            Inode = inode;
            UserId = userId;
            GroupId = groupId;
            FileSize = fileSize;
        }

        internal unsafe static IndexEntry FromNative(git_index_entry* nativeEntry)
        {
            Ensure.ArgumentNotNull(nativeEntry, "nativeEntry");
            Ensure.ArgumentConformsTo(() => nativeEntry->dev <= int.MaxValue, "dev", "dev m");

            int stage = (nativeEntry->flags & git_index.GIT_INDEX_ENTRY_STAGEMASK) >> git_index.GIT_INDEX_ENTRY_STAGESHIFT;

            var flags = IndexEntryFlags.None;
            flags |= (nativeEntry->flags & (ushort)git_index_entry_flag_t.GIT_INDEX_ENTRY_VALID) != 0 ? IndexEntryFlags.Valid : 0;
            flags |= (nativeEntry->extended_flags & (ushort)git_index_entry_extended_flag_t.GIT_INDEX_ENTRY_INTENT_TO_ADD) != 0 ? IndexEntryFlags.IntentToAdd : 0;
            flags |= (nativeEntry->extended_flags & (ushort)git_index_entry_extended_flag_t.GIT_INDEX_ENTRY_SKIP_WORKTREE) != 0? IndexEntryFlags.SkipWorktree : 0;

            return new IndexEntry(
                path: Utf8Converter.FromNative(nativeEntry->path),
                mode: (FileMode)nativeEntry->mode,
                id: ObjectId.FromNative(nativeEntry->id),
                stage: (IndexEntryStage)stage,
                flags: flags,
                changeTime: IndexEntryTime.FromNative(nativeEntry->ctime),
                modificationTime: IndexEntryTime.FromNative(nativeEntry->mtime),
                device: nativeEntry->dev,
                inode: nativeEntry->ino,
                userId: nativeEntry->uid,
                groupId: nativeEntry->gid,
                fileSize: nativeEntry->file_size);
        }

        /// <summary>
        /// Gets the path for the index entry.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the object id for the index entry.
        /// </summary>
        public ObjectId Id { get; private set; }

        /// <summary>
        /// Gets the file mode for the index entry.
        /// </summary>
        public FileMode Mode { get; private set; }

        /// <summary>
        /// Gets the stage level for the index entry.
        /// </summary>
        public IndexEntryStage Stage { get; private set; }

        /// <summary>
        /// Gets the flags for the index entry.
        /// </summary>
        public IndexEntryFlags Flags { get; private set; }

        /// <summary>
        /// Gets the last changed time for the working tree file.
        /// </summary>
        public IndexEntryTime ChangeTime { get; private set; }

        /// <summary>
        /// Gets the last modified time for the working tree file.
        /// </summary>
        public IndexEntryTime ModificationTime { get; private set; }

        /// <summary>
        /// Gets the device number for the working tree file.
        /// </summary>
        public long Device { get; private set; }

        /// <summary>
        /// Gets the inode for the working tree file.
        /// </summary>
        public long Inode { get; private set; }

        /// <summary>
        /// Gets the owner's uid for the working tree file.
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// Gets the owner's gid for the working tree file.
        /// </summary>
        public long GroupId { get; private set; }

        /// <summary>
        /// Gets the size of the working tree file.
        /// </summary>
        public long FileSize { get; private set; }

        private static readonly EqualityHelper<IndexEntry> equalityHelper =
            new EqualityHelper<IndexEntry>(x => x.Path, x => x.Id, x => x.Mode, x => x.Stage);

        public override int GetHashCode()
        {
            return equalityHelper.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            return equalityHelper.Equals(this, obj as IndexEntry);
        }
    }
}
