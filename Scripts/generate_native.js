#!/usr/bin/env node

/*
 * This script produces the `Dogged.Native` PInvoke layer from the
 * libgit2 API documentation.
 *
 * libgit2's API doucmentation is available at
 * https://libgit2.org/docs/api/<version>.json
 */

const fs = require('node:fs/promises');
const process = require('node:process');
const path = require('node:path');

const usings = [ 'System', 'System.Runtime.InteropServices' ];
const namespace = 'Dogged.Native';

const functionsClass = 'libgit2';

const lineLen = 76;
const tab = `    `;

/*
 * Anonymous delegates are translated imperfectly; we can error
 * out instead.
 */
const allowAnonymousDelegates = true;

/* Ignore the "deprecated" group and don't translate them. */
const ignoreGroups = [ 'deprecated' ];

/*
 * We can model an inheritance structure, which allows us to convert
 * between the native types.
 */
const inheritance = {
    'git_commit': 'git_object',
    'git_tree': 'git_object',
    'git_blob': 'git_object',
};

/*
 * Some members have reserved names; we can map them to nicer names
 * than the default conflict avoidance mapping mechanism provides.
 */
const renames = {
    'git_worktree_add_options': {
        'lock': 'should_lock',
        'ref': 'ref_name'
    },
};

/* Type translation to avoid the defaults. */
const types = {
    'git_buf': {
        'ptr': 'byte*',
    },
    'git_error': {
        'message': 'byte*',
    },
    'git_revspec': {
        'from': 'git_object*',
        'to': 'git_object*',
    },
    'git_signature': {
        'name': 'byte*',
        'email': 'byte*',
    },
    'git_index_entry': {
        'path': 'byte*',
    },
    'git_oid': {
        'id': 'unsafe fixed byte[GIT_OID_MAX_SIZE]',
        'GIT_OID_DEFAULT': 'git_oid_t',
    },
    'git_filter_list_apply_to_buffer': {
        'in': 'byte*',
    },
};

/* Value translation to avoid the literal native results. */
const values = {
    'git_oid': {
        'GIT_OID_DEFAULT': 'git_oid_t.GIT_OID_SHA1',
    },
};

/*
 * Resource types that aren't identified as such. These are
 * structures that should be converted to SafeHandles; generally
 * safe handles are all opaque structs that have a free function.
 * Reference iterators are not opaque to allow custom implementation.
 */
const resources = [ 'git_reference_iterator' ];

/*
 * Literal replacements; especially important for things like
 * varargs style functions (eg git_libgit2_opts) that are hard
 * to model and understand in .NET.
 */
const replacements = {
    'git_oid': {
        'GIT_OID_SHA1_ZERO': '',
        'GIT_OID_SHA1_HEXZERO': '',
        'GIT_OID_SHA256_ZERO': '',
        'GIT_OID_SHA256_HEXZERO': '',
    },

    'git_index_reuc_entry': [
        '/// <summary>',
        '///  Representation of a resolve undo entry in the index.',
        '/// </summary>',
        '[StructLayout(LayoutKind.Sequential)]',
        'public unsafe struct git_index_reuc_entry',
        '{',
        '   public uint ancestor_mode;',
        '   public uint our_mode;',
        '   public uint their_mode;',
        '',
        '   public git_oid ancestor_oid;',
        '   public git_oid our_oid;',
        '   public git_oid their_oid;',
        '',
        '   public char* path;',
        '}'
    ],

    'git_libgit2_opts': [
        '/// <summary>',
        '/// Get or set a global configuration option for libgit2.',
        '/// </summary>',
        '/// <param name="option">The option value to get or set</param>',
        '/// <param name="...">The options to set</param>',
        '/// <returns>0 on success or an error code</returns>',
        '',
        '// macOS arm64 requires padding before varargs, we\'ll set up the',
        '// PInvoke methods with the extra padding as private, then use a',
        '// public C# method to proxy to them. This gives us a consistent',
        '// API between macOS arm64 and everything else.',
        '// see https://github.com/dotnet/runtime/issues/48796',
        '#if MACOS_ARM64',
        '   [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]',
        '   private static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, IntPtr padding1, IntPtr padding2, IntPtr padding3, IntPtr padding4, IntPtr padding5, IntPtr padding6, IntPtr padding7, git_config_level_t level, ref git_buf buf);',
        '',
        '   public static int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, ref git_buf buf)',
        '   {',
        '       return git_libgit2_opts(option, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, level, ref buf);',
        '   }',
        '',
        '   [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]',
        '   private static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, IntPtr padding1, IntPtr padding2, IntPtr padding3, IntPtr padding4, IntPtr padding5, IntPtr padding6, IntPtr padding7, git_config_level_t level, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);',
        '',
        '   public static int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, string path)',
        '   {',
        '       return git_libgit2_opts(option, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, level, path);',
        '   }',
        '#else',
        '   [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]',
        '   public static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, ref git_buf buf);',
        '',
        '   [DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]',
        '   public static extern unsafe int git_libgit2_opts(git_libgit2_opt_t option, git_config_level_t level, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))] string path);',
        '#endif',
    ],
};

/* String marshaling functions */
const fromNativeStringMarshal = 'MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.FromNative, MarshalTypeRef = typeof(Utf8Marshaler))';
const toNativeStringMarshal = 'MarshalAs(UnmanagedType.CustomMarshaler, MarshalCookie = Utf8Marshaler.ToNative, MarshalTypeRef = typeof(Utf8Marshaler))';

const missingNameFillers =
    [ 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
      'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' ];


if (!process.argv[2] || !process.argv[3]) {
    process.stderr.write(`usage: ${path.basename(process.argv[1])} documentation.json path_to_output\n`);
    process.exit(1);
}

const sourceFile = process.argv[2];
const outputDir = process.argv[3];
const handlesOutputDir = `${outputDir}/Handles`;


function shouldIgnoreGroup(name) {
    for (const ignored of ignoreGroups) {
        if (name === ignored) {
            return true;
        }
    }

    return false;
}

function getIndent(data) {
    let indent = "";

    for (let i = 0; i < data?.indent; i++) {
        indent += `${tab}`;
    }

    return indent;
}

function wrap(text, data) {
    let remain = text;
    const indent = getIndent(data);
    const result = [ ];
    let wrapLen = lineLen - indent.length;

    while (remain.length > wrapLen) {
        let splitPosition = wrapLen;

        while (splitPosition > 0) {
            if (remain.charAt(splitPosition--) == ' ') {
                break;
            }
        }

        if (!splitPosition) {
            // we have an overly long line. try advancing forward
            // to split at the first whitespace
            splitPosition = wrapLen;

            while (splitPosition < remain.length) {
                if (remain.charAt(splitPosition + 1) == ' ') {
                    break;
                }

                splitPosition++;
            }

            if (splitPosition == remain.length) {
                // this is an unwrappable long line; stop
                break;
            }
        }

        result.push(remain.slice(0, splitPosition + 1));

        while (splitPosition < remain.length) {
            if (remain.charAt(splitPosition + 2) != ' ') {
                break;
            }

            splitPosition++;
        }

        remain = remain.slice(splitPosition + 2);
    }

    if (remain.length) {
        result.push(remain);
    }

    return result;
}

function produceDescriptionComment(decl, data) {
    let result = "";
    let indent = getIndent(data);
    let count = 0;

    if (decl.comment) {
        result += `${indent}/// <summary>\n`;

        for (para of Array.isArray(decl.comment) ? decl.comment : [ decl.comment ]) {
            if (count++ > 0) {
                result += `${indent}///\n`;
                result += `${indent}///  <para>\n`;
            }

            para = para.replaceAll(/\s+/g, ' ');

            for (const line of wrap(para, { indent: data?.indent + 2 + (count > 1 ? 1 : 0) } )) {
                result += `${indent}/// ${count > 1 ? ' ' : ''} ${line}\n`;
            }

            if (count > 1) {
                result += `${indent}///  </para>\n`;
            }
        }

        result += `${indent}/// </summary>\n`;
    }

    return result;
}

function produceEnumData(decl, data) {
    let result = "";
    let indent = getIndent(data);
    let count = 0;

    result += produceDescriptionComment(decl, data);

    const fullType = decl.name.startsWith('enum ') ? decl.name : `enum ${decl.name}`;

    if (decl.subkind === 'flags') {
        result += `${indent}[Flags]\n`;
    }

    result += `${indent}public ${fullType}\n`;
    result += `${indent}{\n`;

    let packed = (decl.members.filter(m => m.comment !== undefined).length == 0);

    for (const member of decl.members) {
        if (count++ > 0 && !packed) {
            result += `\n`;
        }

        result += produceDescriptionComment(member, { indent: data.indent + 1 });
        result += `${indent}${tab}${member.name}`;

        if (member.value !== undefined) {
            result += ` = ${member.value}`;
        }

        result += `,\n`;
    }

    result += `${indent}}\n`;

    return result;
}

function convertDelegateParamType(decl, param, data) {
    return convertFunctionParamType(decl, param, data);
}

function convertDelegateParamName(delegateName, paramName, data) {
    return convertFunctionOrDelegateParamName('function pointer delegate', delegateName, paramName, data);
}

function convertDelegateReturnType(decl, param, data) {
    return convertFunctionReturnType(decl, param, data);
}

function parseFunctionPointer(type) {
    const signature = type.match(/^([A-Za-z0-9_]+\s*\**)\(\*\)\((.*)\)$/);
    const [ , returnType, paramData ] = signature;
    const params = paramData.split(/,\s+/);

    return [ returnType.trim(), params ];
}

function produceAnonymousDelegate(decl, data) {
    let result = "";
    let indent = getIndent(data);

    const [ returnType, params ] = parseFunctionPointer(decl.referenceType);

    result += produceDescriptionComment(decl, data);

    result += `${indent}[UnmanagedFunctionPointer(CallingConvention.Cdecl)]\n`;
    result += `${indent}public unsafe delegate `;
    result += convertFunctionReturnType(decl, returnType, data);
    result += ` ${decl.type}(`;

    for (const i in params) {
        const paramType = params[i];

        if (i > 0) {
            result += `,`;
        }

        result += `\n`;
        result += `${indent}${tab}${convertDelegateParamType(decl, { type: paramType }, data)} ${missingNameFillers[i]}`;
    }

    result += `);\n`;

    return result;
}

function produceDelegateData(decl, data) {
    let result = "";
    let indent = getIndent(data);

    result += produceFunctionComments(decl, data);

    result += `${indent}[UnmanagedFunctionPointer(CallingConvention.Cdecl)]\n`;
    result += `${indent}public unsafe delegate `;
    result += convertFunctionReturnType(decl,
        decl.returns?.type ?? parseFunctionPointer(decl.type)[0],
        data);
    result += ` ${decl.name}(`;

    for (const i in decl.params) {
        const param = decl.params[i];

        if (i > 0) {
            result += `,`;
        }

        result += `\n`;
        result += `${indent}${tab}${convertDelegateParamType(decl, param, data)} ${convertDelegateParamName(decl.name, param.name, data)}`;
    }

    result += `);\n`;

    return result;
}

function unpackType(t) {
    const m = t.match(/^(.*)(\[[^\]]+\])$/);

    if (m) {
        return [ m[1], m[2] ];
    }

    return [ t, '' ];
}

function produceStructData(decl, data) {
    let shortType = undefined;
    let result = "";
    let indent = getIndent(data);
    let count = 0;

    result += produceDescriptionComment(decl, data);

    let structName = decl.name.replace(/^struct /, "");
    let fullType = `struct ${structName}`;

    if (decl.opaque) {
        result += `${indent}public ${fullType}\n`;
    }
    else {
        result += `${indent}[StructLayout(LayoutKind.Sequential)]\n`;
        result += `${indent}public unsafe ${fullType}\n`;
    }

    result += `${indent}{\n`;

    if (decl.subkind === 'options') {
        if (!decl.initFunction) {
            throw new Error("options declared without corresponding init_function");
        }
        if (!decl.versionMacro) {
            throw new Error("options declared without corresponding version");
        }

        const currentVersion = data.decls.macros.filter(m => m.name === decl.versionMacro)[0].value;

        result += `${indent}${tab}/// <summary>\n`;
        result += `${indent}${tab}/// Current version of the options structure.\n`;
        result += `${indent}${tab}/// </summary>\n`;
        result += `${indent}${tab}public static readonly uint ${decl.versionMacro} = ${currentVersion};\n\n`;

        count++;
    }

    for (const macro of decl.macros ? decl.macros : [ ]) {
        let replacement = produceReplacement(macro, data);

        if (replacement === '') {
            continue;
        }

        if (count++ > 0) {
            result += `\n`;
        }

        result += produceDescriptionComment(macro, { indent: data.indent + 1 });
        result += replacement ? replacement :
                  produceMacroData(macro, { indent: data.indent + 1 });
    }

    for (const member of decl.opaque ? [ ] : decl.members) {
        let memberType = convertMemberType(decl, member, data);
        let memberName = convertMemberName(decl, member);

        if (count++ > 0) {
            result += `\n`;
        }

        [ memberType, memberTypeSuffix ] = unpackType(memberType);

        result += produceDescriptionComment(member, { indent: data.indent + 1 });

        result += `${indent}${tab}public ${memberType} ${memberName}${memberTypeSuffix};\n`;
    }

    result += `${indent}}\n`;

    return result;
}

function produceMacroData(decl, data) {
    let result = "";
    let indent = getIndent(data);
    let type = undefined;
    let value = decl.value;
    let constness = 'const';

    let redefinedValue = lookupValue({ name: `git_${decl.group}` }, decl);
    let redefinedType = lookupType({ name: `git_${decl.group}` }, decl);

    if (redefinedValue) {
        value = redefinedValue;
    }
    else if (decl.value === 'UINT64_MAX') {
        value = 'ulong.MaxValue';
        redefinedType = redefinedType || 'ulong';
    }
    else if (decl.value === 'SIZE_MAX') {
        value = '(IntPtr.Size == 8 ? ulong.MaxValue : (ulong)uint.MaxValue)';
        redefinedType = redefinedType || 'ulong';
        constness = 'static readonly';
    }

    if (redefinedType) {
        type = redefinedType;
    }
    else if (decl.value.match(/^".*"$/)) {
        type = 'string';
    }
    else if (decl.value.match(/^-?\d+\.\d+$/)) {
        type = 'float';
    }
    else if (decl.value.match(/u$/)) {
        type = 'uint';
    }
    else {
        type = 'int';
    }

    result += `${indent}public ${constness} ${type} ${decl.name} = ${value};\n`;
    return result;
}

function commentStartTag(tag) {
    let result = "";

    if (!Array.isArray(tag)) {
        tag = [ tag ];
    }

    result += `<`;
    for (const i in tag) {
        if (i > 0) {
            result += ` `;
        }

        result += `${tag[i]}`;
    }
    result += `>`;

    return result;
}

function commentEndTag(tag) {
    return `</${Array.isArray(tag) ? tag[0] : tag}>`;
}

function prettifyComment(tag, comment, data) {
    let result = "";
    let indent = getIndent(data);

    comment = comment.replace(/\s+/g, ' ');

    const startTag = commentStartTag(tag);
    const endTag = commentEndTag(tag);

    result += `${indent}/// ${startTag}`;

    if (result.length + comment.length + endTag.length <= lineLen) {
        result += `${comment}${endTag}\n`;
    }
    else {
        result += `\n`;

        for (const line of wrap(comment, { indent: data?.indent + 2 } )) {
            result += `${indent}///  ${line}\n`;
        }

        result += `${indent}/// ${endTag}\n`;
    }

    return result;
}

function produceFunctionComments(decl, data) {
    let result = "";
    let indent = getIndent(data);
    let count = 0;

    result += produceDescriptionComment(decl, data);

    for (const param of (decl.params ? decl.params : [ ])) {
        if (param.comment !== undefined) {
            result += prettifyComment([ 'param', `name="${convertFunctionParamName(decl.name, param.name, data)}"` ], param.comment, data);
        }
    }

    if (decl.returns?.comment) {
        result += prettifyComment('returns', decl.returns.comment, data);
    }

    return result;
}

function isStruct(typeName, data) {
    for (const struct of data.global.structs) {
        if (struct.name === typeName) {
            return true;
        }
    }

    return false;
}

function isFreeFunction(typeName, data) {
    return typeName.endsWith('_free');
}

function isManagedStruct(typeName, data) {
    for (const struct of data.global.structs) {
        if (struct.name === typeName && !struct.opaque) {
            return true;
        }
    }

    return false;
}

function structHasManagedMembers(typeName, data) {
    for (const struct of data.global.structs) {
        if (struct.name === typeName && !struct.opaque) {
            for (const m of struct.members) {
                if (isFunctionPointer(m.type) ||
                    (m.referenceType && isFunctionPointer(m.referenceType))) {
                    return true;
                }
            }
        }
    }

    return false;
}

function convertType(type, data) {
    /* On the C side we don't always create typedefs, but in .NET these
     * are always `struct`s and get referred to without the `struct`
     * prefix. Remove the struct prefix if it exists.
     */
    if (type.startsWith('struct ') &&
        (ourTypeName = type.replace(/^struct ([a-zA-Z0-9_]+).*$/, '$1')) &&
        isStruct(ourTypeName, data)) {

        type = type.replace(/^struct\s+/, '');
    }

    // Pinvoke doesn't understand "const" yet
    type = type.replace(/^const\s+/, '')
        .replace(/\*\s*const\s*\*/, '');

    if (type === 'unsigned short' || type === 'uint16_t') {
        return 'ushort';
    }
    else if (type === 'int' || type === 'int32_t') {
        return 'int';
    }
    else if (type === 'unsigned int' || type === 'uint32_t') {
        return 'uint';
    }
    else if (type === 'unsigned int *' || type === 'uint32_t *') {
        return 'uint*';
    }
    else if (type === 'int64_t') {
        return 'long';
    }
    else if (type === 'uint64_t') {
        return 'ulong';
    }
    else if (type === 'void *' || type === 'void **') {
        return 'IntPtr';
    }
    else if (type === 'ssize_t') {
        return 'IntPtr';
    }
    else if (type === 'size_t') {
        return 'UIntPtr';
    }
    else if (type === 'size_t *') {
        return 'UIntPtr*';
    }
    else if (type === 'git_object_size_t') {
        return `long`;
    }
    else if (type === 'git_time_t') {
        return 'long';
    }
    else if (type === 'git_off_t') {
        return 'IntPtr';
    }
    else if (type === 'unsigned char *') {
        return `byte*`;
    }
    else if (type === 'char *') {
        return `byte*`;
    }
    else if (type.match(/^(unsigned )?char\[\d+\]/)) {
        return `byte*`;
    }
    else if (type === 'unsigned char') {
        return `byte`;
    }

    return type.replace(/ (\*+)$/, "$1");
}

function convertMemberType(parentDecl, member, data) {
    let type = member.extendedType?.type || member.type;
    let redefined = lookupType(parentDecl, member);
        
    if (redefined) {
        return redefined;
    }

    if (isFunctionPointer(type)) {
        return 'IntPtr';
    }

    if (type.endsWith(' *')) {
        return 'IntPtr';
    }

    return convertType(type, data).replace(/ (\*+)$/, "$1");
}

function lookupValue(parentDecl, decl) {
    return values[parentDecl.name] ?
           values[parentDecl.name][decl.name] :
           undefined;
}

function lookupType(parentDecl, decl) {
    return types[parentDecl.name] ?
           types[parentDecl.name][decl.name] :
           undefined;
}

function lookupRename(parentDecl, decl) {
    return renames[parentDecl.name] ?
           renames[parentDecl.name][decl.name] :
           undefined;
}

function isReservedName(name) {
    if (name === 'out' || name === 'in' || name === 'ref' ||
        name === 'object' || name === 'lock' || name === 'string' ||
        name === 'abstract' || name === 'base' || name === 'readonly' ||
        name === 'foreach') {
        return true;
    }

    return false;
}

function convertMemberName(parentDecl, childDecl) {
    let renamed = lookupRename(parentDecl, childDecl);

    if (renamed) {
        return renamed;
    }

    if (isReservedName(childDecl.name)) {
        process.stderr.write(`warning: ${parentDecl.kind} ${parentDecl.name} has parameter named \`${childDecl.name}\`, renaming to \`_${childDecl.name}\`\n`);
        return `_${childDecl.name}`;
    }

    return childDecl.name;
}

function convertFunctionReturnType(parentDecl, type, data) {
    let redefined = lookupType(parentDecl, { name: "return" });
    const bareName = type.replace(/^(?:const )?(?:struct )?(.*)\s+\*+$/, "$1");
        
    if (redefined) {
        return redefined;
    }

    if (type === 'const char *') {
        type = 'string';
    }
    else if (type === 'git_error_code') {
        type = 'int';
    }
    else if (type === 'bool') {
        type = 'int';
    }
    else if (structHasManagedMembers(bareName, data)) {
        type = 'IntPtr';
    }
    else {
        type = convertType(type, data);
    }

    type = type.replace(/ (\*+)$/, "$1");

    return type;
}

function functionExists(name, data) {
    for (const decl of data.global.functions.filter(d => d.name === name)) {
        return true;
    }

    return false;
}

function shouldTranslateStructResource(decl, data) {
    for (const r of resources) {
        if (r === decl.name) {
            return true;
        }
    }

    return decl.opaque && functionExists(`${decl.name}_free`, data);
}

function convertFunctionHandlerOverloadType(parentDecl, param, data) {
    let type = param.extendedType?.type !== undefined ?
               param.extendedType.type : param.type;
    let out = false;

    if (param.direction === 'out') {
        if (!type.endsWith('*')) {
            throw new Error("out param is not a pointer");
        }

        type = type.slice(0, -1);
        out = true;
    }

    const bareName = type.replace(/^(?:const )?(?:struct )?(.*)\s+\*+$/, "$1");

    for (const struct of data.global.structs) {
        if (struct.name === bareName && shouldTranslateStructResource(struct, data)) {
            const handleName = produceHandleName(struct, data);

            let result = out ? 'out ' : '';
            result += handleName;

            return result;
        }
    }

    return undefined;
}

function convertFunctionParamType(parentDecl, param, data) {
    let type = param.extendedType?.type !== undefined ?
               param.extendedType.type : param.type;
    let out = false;
    let marshal = undefined;
    let ourTypeName = undefined;

    if (type.startsWith('out '))
        throw new Error(type);

    let redefined = lookupType(parentDecl, param);
        
    if (redefined) {
        return redefined;
    }

    if (param.direction === 'out') {
        if (!type.endsWith('*')) {
            throw new Error("out param is not a pointer");
        }

        type = type.slice(0, -1);
        out = true;
    }

    const bareName = type.replace(/^(?:const )?(?:struct )?(.*)\s+\*+$/, "$1");

    type = type.trim();

    if (!out && type.endsWith(' *') &&
        (ourTypeName = type.replace(/^(const )?(struct )?([A-Za-z0-9_]+)\s*\*$/, '$3')) &&
        isManagedStruct(ourTypeName, data)) {
        type = `ref ${ourTypeName}`;
    } else if (!out && type.endsWith(' *') &&
        (ourTypeName = type.replace(/^(const )?(struct )?([A-Za-z0-9_]+)\s*\*$/, '$3')) &&
        isStruct(ourTypeName, data)) {
        type = `${ourTypeName}*`;
    } else if (type.endsWith('**') &&
        (ourTypeName = type.replace(/^(const )?(struct )?([A-Za-z0-9_]+)\s*\*+$/, '$3')) &&
        isManagedStruct(ourTypeName, data)) {
        type = `IntPtr`;
    }
    else if (type === 'const char *' || type === 'char *') {
        marshal = out ?  `[${fromNativeStringMarshal}]` : `[${toNativeStringMarshal}]`;
        type = 'string';
    }
    else {
        type = convertType(type, data);
    }

    if (out) {
        type = `out ${type}`;
    }

    if (marshal) {
        type = `${marshal} ${type}`;
    }

    return type;
}

function getMissingParamName(kind, name, paramName, data) {
    let idx = data.missingParamNames[name] || 0;
    data.missingParamNames[name] = idx + 1;

    return missingNameFillers[idx];
}

function convertFunctionOrDelegateParamName(kind, name, paramName, data) {
    let renamed = lookupRename({ kind: kind, name: name }, { name: paramName});

    if (renamed) {
        return renamed;
    }

    if (!paramName) {
        paramName = getMissingParamName(kind, name, paramName, data);
    }

    if (isReservedName(paramName)) {
        process.stderr.write(`warning: ${kind} ${name} has parameter named \`${paramName}\`, renaming to \`_${paramName}\`\n`);
        return `_${paramName}`;
    }

    return paramName;
}

function convertFunctionParamName(functionName, paramName, data) {
    return convertFunctionOrDelegateParamName('function', functionName, paramName, data);
}

function typeIsUsed(name, data) {
    for (const struct of data.global.structs) {
        if (struct.name === name) {
            return true;
        }

        if (!struct.opaque &&
            struct.members.filter(p => p.type === name).length > 0) {
            return true;
        }
    }

    for (const fn of data.global.functions) {
        if (fn.name === name) {
            return true;
        }

        if ((fn.params || []).filter(p => p.type === name).length > 0) {
            return true;
        }
    }

    if (data.global.macros.filter(m => m.name === name).length > 0 ||
        data.global.typedefs.filter(t => t.name === name).length > 0) {
        return true;
    }

    return false;
}

function prepareAnonymousDelegatesForDecls(decl, children, converter, data) {
    let result = "";
    let count = 0;

    for (const child of children || [ ]) {
        const convertedType = converter(decl, child, data);

        if (!isFunctionPointer(convertedType) || child.referenceType !== undefined) {
            continue;
        }

        if (!allowAnonymousDelegates) {
            throw new Error(`${decl.name} contains anonymous function pointers`);
        }

        // Generate a name for this delegate.
        const baseName = `${decl.name}_cb`;
        let delegateName = baseName;
        let num = 1;

        while (typeIsUsed(delegateName, data)) {
            num++;
            delegateName = `${baseName}_${num}`;
        }

        console.log(`warning: ${decl.kind} \`${decl.name}\` has non-typedef'd function pointer, creating delegate \`${delegateName}\``);

        child.referenceType = child.type;
        child.type = delegateName;

        // add this to results
        if (count++ > 0) {
            result += '\n';
        }

        result += produceAnonymousDelegate(child, data);
    }

    return result;
}

function prepareAnonymousDelegatesForStruct(decl, data) {
    return prepareAnonymousDelegatesForDecls(decl, decl.members, convertMemberType, data);
}

function prepareAnonymousDelegatesForFunction(decl, data) {
    return prepareAnonymousDelegatesForDecls(decl, decl.params, convertFunctionParamType, data);
}

function produceFunctionData(decl, data) {
    let result = "";
    let indent = getIndent(data);

    const returnType = convertFunctionReturnType(decl,
        decl.returns.extendedType?.type || decl.returns.type,
        data);

    result += produceFunctionComments(decl, data);
    result += `${indent}[DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]\n`;

    if (returnType === 'string') {
        result += `${indent}[return: ${fromNativeStringMarshal}]\n`;
    }

    result += `${indent}public static extern unsafe ${returnType} ${decl.name}(`;

    for (const i in decl.params) {
        const param = decl.params[i];
        const paramType = convertFunctionParamType(decl, param, data);

        if (i > 0) {
            result += `,`;
        }

        result += `\n`;
        result += `${indent}${tab}${paramType} ${convertFunctionParamName(decl.name, param.name, data)}`;
    }

    result += `);`;

    return result;
}

function isFunctionPointer(type) {
    return type.match(/^(?:const )?[A-Za-z0-9_]+\s*\**\(\*/);
}

function prepareAnonymousDelegates(decls, data) {
    let result = '';
    let count = 0;

    /* Struct members */
    for (const decl of decls.all) {
        let delegateData = undefined;

        switch (decl.kind) {
            case 'struct':
                if (decl.opaque) {
                    continue;
                }

                delegateData = prepareAnonymousDelegatesForStruct(decl, data);
                break;
            case 'function':
                delegateData = prepareAnonymousDelegatesForFunction(decl, data);
                break;
            default:
                continue;
        }

        if (delegateData !== undefined && delegateData.length > 0) {
            if (count++ > 0) {
                result += '\n';
            }

            result += delegateData;
        }
    }

    if (count > 0) {
        result += '\n';
    }

    return result;
}

function produceReplacement(decl, data) {
    let indent = getIndent(data);
    let replacement = undefined;

    if (decl.kind === 'macro') {
        replacement = replacements[`git_${decl.group}`]?.[decl.name];
    } else {
        replacement = replacements[decl.name];
    }

    if (typeof replacement === 'string') {
        return replacement;
    }

    if (Array.isArray(replacement)) {
        let result = '';

        for (const line of replacements[decl.name]) {
            result += indent + line + '\n';
        }

        return result;
    }

    return undefined;
}

function isObjectMacro(decl) {
    return decl.name.match(/^([A-Za-z0-9_]+)$/);
}

function isOptionsMacro(decl) {
    return decl.name.endsWith('_INIT') || decl.name.endsWith('_VERSION');
}

function producePinvokeForDecls(file, data) {
    let result = '';
    let count = 0, functionCount = 0;

    data.indent = 1;

    for (const using of usings) {
        result += `using ${using};\n`;
    }
    result += `\n`;

    result += `namespace ${namespace}\n`;
    result += `{\n`;

    result += prepareAnonymousDelegates(data.decls, data);

    /*
     * If there is a struct named for the group (eg, `git_oid` in
     * the `oid` group), bring it to the top and fill in any macros
     */
    for (const decl of data.decls.all.filter(d => (d.kind === 'struct' && d.name === `git_${file}`))) {
        if (count++ > 0) {
            result += '\n';
        }

        decl.macros = data.decls.all.filter(d => (d.kind === 'macro' && isObjectMacro(d) && !isOptionsMacro(d)));

        result += produceStructData(decl, data);
    }

    /* Everything but functions or macros */
    for (const decl of data.decls.all.filter(d => (d.kind !== 'function' && d.kind !== 'macro'))) {
        let replacement = produceReplacement(decl, data);

        if (replacement === '') {
            continue;
        } else if (replacement) {
            result += ((count++ > 0) ? '\n' : '') + replacement;
            continue;
        }

        switch (decl.kind) {
            case 'enum':
                if (count++ > 0) {
                    result += '\n';
                }

                result += produceEnumData(decl, data);
                break;

            case 'struct':
                if (decl.name === `git_${file}`) {
                    continue;
                }

                if (count++ > 0) {
                    result += '\n';
                }

                result += produceStructData(decl, data);
                break;

            case 'typedef':
                if (!isFunctionPointer(decl.referenceType || decl.type)) {
                    continue;
                }

                if (count++ > 0) {
                    result += '\n';
                }

                result += produceDelegateData(decl, data);
                break;
            default:
                throw new Error(`unknown declaration type ${decl.kind}`);
        }
    }

    /* Functions */

    data.indent++;

    for (const decl of data.decls.functions) {
        if (++functionCount == 1) {
            if (count++ > 0) {
                result += '\n';
            }

            result += `${tab}public static partial class ${functionsClass}\n`;
            result += `${tab}{\n`;
        } else {
            result += '\n';
        }

        let replacement = produceReplacement(decl, data);

        if (replacement === '') {
            continue;
        }

        result += replacement ? replacement :
                  produceFunctionData(decl, data);
        result += `\n`;
    }

    if (functionCount > 0) {
        result += `${tab}}\n`;
    }

    result += `}\n`;

    return result;
}

function uppercaseStructName(decl, data) {
    return decl.name.replace(/^git/, '')
        .replace(/_([a-z])/g, (_, c) => c.toUpperCase());
}

function produceHandleName(decl, data) {
    return uppercaseStructName(decl, data) + `Handle`;
}

function produceStructHandleParent(decl, data) {
    const baseClass = inheritance[decl.name];

    return baseClass !== undefined ? produceHandleName({ name: baseClass }, data) : undefined;
}

function isBaseClass(decl, data) {
    for (const base of Object.values(inheritance)) {
        if (base === decl.name) {
            return true;
        }
    }

    return false;
}

function produceStructHandlerWrapper(decl, data) {
    let result = '';
    let indent = getIndent(data);

    const structName = uppercaseStructName(decl, data);
    const handleName = produceHandleName(decl, data);

    const baseObject = inheritance[decl.name];
    const baseClass = baseObject !== undefined ?produceStructHandleParent(decl, data) : 'SafeHandle';

    const sealed = isBaseClass(decl, data) ? '' : 'sealed ';

    result += `${indent}public ${sealed}class ${handleName} : ${baseClass}\n`;
    result += `${indent}{\n`;

    if (baseObject !== undefined) {
        result += `${indent}${tab}public ${handleName}() : base()\n`;
    } else {
        result += `${indent}${tab}public ${handleName}() : base(IntPtr.Zero, true)\n`;
    }

    result += `${indent}${tab}{\n`;
    result += `${indent}${tab}}\n`;
    result += `\n`;
    result += `${indent}${tab}public unsafe ${handleName}(${decl.name}* native${structName}) :\n`;

    if (baseObject !== undefined) {
        result += `${indent}${tab}${tab}base((${baseObject}*)native${structName})\n`;
    } else {
        result += `${indent}${tab}${tab}base((IntPtr)native${structName}, true)\n`;
    }

    result += `${indent}${tab}{\n`;
    result += `${indent}${tab}}\n`;
    result += `\n`;
    result += `${indent}${tab}public override bool IsInvalid\n`;
    result += `${indent}${tab}{\n`;
    result += `${indent}${tab}${tab}get\n`;
    result += `${indent}${tab}${tab}{\n`;
    result += `${indent}${tab}${tab}${tab}return handle == IntPtr.Zero;\n`;
    result += `${indent}${tab}${tab}}\n`;
    result += `${indent}${tab}}\n`;
    result += `\n`;
    result += `${indent}${tab}protected override unsafe bool ReleaseHandle()\n`;
    result += `${indent}${tab}{\n`;
    result += `${indent}${tab}${tab}libgit2.${decl.name}_free(handle);\n`;
    result += `${indent}${tab}${tab}handle = IntPtr.Zero;\n`;
    result += `${indent}${tab}${tab}return true;\n`;
    result += `${indent}${tab}}\n`;
    result += `${indent}}\n`;
;

    return result;
}

function produceFunctionHandlerOverload(decl, data) {
    let result = "";
    let indent = getIndent(data);
    let translatedParams = 0;

    const returnType = convertFunctionReturnType(decl,
        decl.returns.extendedType?.type || decl.returns.type,
        data);

    result += produceFunctionComments(decl, data);
    result += `${indent}[DllImport(libgit2_dll, CallingConvention = CallingConvention.Cdecl)]\n`;

    if (returnType === 'string') {
        result += `${indent}[return: ${fromNativeStringMarshal}]\n`;
    }

    result += `${indent}public static extern unsafe ${returnType} ${decl.name}(`;

    for (const i in decl.params) {
        const param = decl.params[i];
        let paramType = convertFunctionHandlerOverloadType(decl, param, data);

        if (isFreeFunction(decl.name, data)) {
            paramType = 'IntPtr';
            translatedParams++;
        } else if (paramType !== undefined) {
            translatedParams++;
        } else {
            paramType = convertFunctionParamType(decl, param, data);
        }

        if (i > 0) {
            result += `,`;
        }

        result += `\n`;
        result += `${indent}${tab}${paramType} ${convertFunctionParamName(decl.name, param.name, data)}`;
    }

    result += `);`;

    return translatedParams > 0 ? result : undefined;
}

function produceHandlerOverloadsForDecls(file, data) {
    let result = '';
    let structCount = 0, functionCount = 0;

    data.indent = 1;

    for (const using of usings) {
        result += `using ${using};\n`;
    }
    result += `\n`;

    result += `namespace ${namespace}\n`;
    result += `{\n`;

    /* Create handler wrappers for structs first */
    for (const decl of data.decls.all.filter(d => (d.kind === 'struct' && shouldTranslateStructResource(d, data)))) {
        if (++structCount > 1) {
            result += `\n`;
        }

        result += produceStructHandlerWrapper(decl, data);
    }

    data.indent++;

    for (const decl of data.decls.functions) {
        const functionOverload = produceFunctionHandlerOverload(decl, data);

        if (functionOverload === undefined) {
            continue;
        }

        if (++functionCount == 1) {
            if (structCount > 0) {
                result += '\n';
            }

            result += `${tab}public static partial class ${functionsClass}\n`;
            result += `${tab}{\n`;
        } else {
            result += '\n';
        }

        result += functionOverload;
        result += `\n`;
    }

    data.indent--;

    if (functionCount > 0) {
        result += `${tab}}\n`;
    }

    result += `}\n`;

    return (structCount > 0 || functionCount > 0) ? result : undefined;
}

function normalizeGroup(groupData) {
    const all = Object.values(groupData.apis).map(item => {
        if (item.kind === 'callback' || item.kind === 'alias') {
            return { ...item, kind: 'typedef' };
        }
        return item;
    });

    return {
        all,
        structs: all.filter(d => d.kind === 'struct'),
        functions: all.filter(d => d.kind === 'function'),
        macros: all.filter(d => d.kind === 'macro'),
        typedefs: all.filter(d => d.kind === 'typedef'),
    };
}

async function producePinvokeForGroup(file, data) {
    const filename = `${outputDir}/${file}.cs`;
    const results = producePinvokeForDecls(file, data);

    await fs.mkdir(outputDir, { recursive: true });
    await fs.mkdir(`${outputDir}/sys`, { recursive: true });
    await fs.writeFile(filename, results);
}

async function produceHandlerOverloadsForGroup(file, data) {
    const filename = `${handlesOutputDir}/${file}.cs`;
    const results = produceHandlerOverloadsForDecls(file, data);

    if (results !== undefined) {
        await fs.mkdir(handlesOutputDir, { recursive: true });
        await fs.mkdir(`${handlesOutputDir}/sys`, { recursive: true });
        await fs.writeFile(filename, results);
    }
}

function flattenGroups(apiData) {
    const all = Object.values(apiData.groups)
                      .flatMap(g => Object.values(g.apis))
                      .map(item => {
        if (item.kind === 'callback' || item.kind === 'alias') {
            return { ...item, kind: 'typedef' };
        }
        return item;
    });

    return {
        all,
        structs: all.filter(d => d.kind === 'struct'),
        functions: all.filter(d => d.kind === 'function'),
        macros: all.filter(d => d.kind === 'macro'),
        typedefs: all.filter(d => d.kind === 'typedef'),
    };
}

async function producePinvoke(apiData) {
    const allData = flattenGroups(apiData);

    for (const group in apiData.groups) {
        if (shouldIgnoreGroup(group)) {
            continue;
        }

        const groupData = normalizeGroup(apiData.groups[group]);
        const data = { decls: groupData, global: allData, missingParamNames: { } };

        await producePinvokeForGroup(group, data);
        await produceHandlerOverloadsForGroup(group, data);
    }
}

(async () => {
    try {
        let apiText;

        if (sourceFile.startsWith('http://') ||
            sourceFile.startsWith('https://')) {
            const response = await fetch(sourceFile);
            apiText = await response.text();
        }
        else {
            apiText = await fs.readFile(sourceFile);
        }

        const apiData = JSON.parse(apiText);
        await producePinvoke(apiData);
    } catch (e) {
        console.error(e);
        process.exit(1);
    }
})();
