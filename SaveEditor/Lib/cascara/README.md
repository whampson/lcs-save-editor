[![Build Status](https://travis-ci.org/whampson/cascara.svg?branch=master)](https://travis-ci.org/whampson/cascara)

(needs updating)
# Binary File Template Specification
## What is it?
**BFT** is a specification for an XML-based binary file template, which is used
for mapping out the structure of binary file formats. A binary file template can
be used used to easily read, create, and manipulate binary files.

BFT allows for the format of a binary file to be laid out as a structured XML
document, known as a *binary file template*. Within a template, identifiers may
be given to structures that found within the binary file format. These
identifiers may be used to read and set values within a binary file of the
target format file after the template has been processed and applied to it. 

## Template Layout
### Overview
(TODO)
### Primitive Data Types
(TODO: Description)
#### List of Primitive Types
<table>
  <tbody>
    <tr>
      <th>Name</th>
      <th>Size (bytes)</th>
      <th>Notes</th>
    </tr>
    <tr>
      <td><code>bool</code>
      </td><td>1</td>
      <td>
        8-bit Boolean value; zero is treated as <code>false</code>, nonzero is
        treated as <code>true</code>
      </td>
    </tr>
    <tr>
      <td><code>bool8</code>
      </td><td>1</td>
      <td>8-bit Boolean value; same rules as <code>bool</code></td>
    </tr>
    <tr>
      <td><code>bool16</code>
      </td><td>2</td>
      <td>16-bit Boolean value; same rules as <code>bool</code></td>
    </tr>
    <tr>
      <td><code>bool32</code>
      </td><td>4</td>
      <td>32-bit Boolean value; same rules as <code>bool</code></td>
    </tr>
    <tr>
      <td><code>bool64</code>
      </td><td>8</td>
      <td>64-bit Boolean value; same rules as <code>bool</code></td>
    </tr>
    <tr>
      <td><code>char</code>
      </td><td>1</td>
      <td>unsigned 8-bit integer; intended for use with characters</td>
    </tr>
    <tr>
      <td><code>char8</code>
      </td><td>1</td>
      <td>unsigned 8-bit integer; intended for use with characters</td>
    </tr>
    <tr>
      <td><code>char16</code>
      </td><td>2</td>
      <td>unsigned 16-bit integer; intended for use with characters</td>
    </tr>
    <tr>
      <td><code>char32</code>
      </td><td>4</td>
      <td>unsigned 32-bit integer; intended for use with characters</td>
    </tr>
    <tr>
      <td><code>char64</code>
      </td><td>8</td>
      <td>unsigned 64-bit integer; intended for use with characters</td>
    </tr>
    <tr>
      <td><code>double</code>
      </td><td>8</td>
      <td>double-precision IEEE 754 floating-point number</td>
    </tr>
    <tr>
      <td><code>dword</code>
      </td><td>4</td>
      <td>unsigned 32-bit integer</td>
    </tr>
    <tr>
      <td><code>float</code>
      </td><td>4</td>
      <td>single-precision IEEE 754 floating-point number</td>
    </tr>
    <tr>
      <td><code>int</code>
      </td><td>4</td>
      <td>signed 32-bit integer</td>
    </tr>
    <tr>
      <td><code>int8</code>
      </td><td>1</td>
      <td>signed 8-bit integer</td>
    </tr>
    <tr>
      <td><code>int16</code>
      </td><td>2</td>
      <td>signed 16-bit integer</td>
    </tr>
    <tr>
      <td><code>int32</code>
      </td><td>4</td>
      <td>signed 32-bit integer</td>
    </tr>
    <tr>
      <td><code>int64</code>
      </td><td>8</td>
      <td>signed 64-bit integer</td>
    </tr>
    <tr>
      <td><code>long</code>
      </td><td>8</td>
      <td>signed 64-bit integer</td>
    </tr>
    <tr>
      <td><code>qword</code>
      </td><td>8</td>
      <td>unsigned 64-bit integer</td>
    </tr>
    <tr>
      <td><code>short</code>
      </td><td>2</td>
      <td>signed 16-bit integer</td>
    </tr>
    <tr>
      <td><code>uint</code>
      </td><td>4</td>
      <td>unsigned 32-bit integer</td>
    </tr>
    <tr>
      <td><code>uint8</code>
      </td><td>1</td>
      <td>unsigned 8-bit integer</td>
    </tr>
    <tr>
      <td><code>uint16</code>
      </td><td>2</td>
      <td>unsigned 16-bit integer</td>
    </tr>
    <tr>
      <td><code>uint32</code>
      </td><td>4</td>
      <td>unsigned 32-bit integer</td>
    </tr>
    <tr>
      <td><code>uint64</code>
      </td><td>8</td>
      <td>unsigned 64-bit integer</td>
    </tr>
    <tr>
      <td><code>ulong</code>
      </td><td>8</td>
      <td>unsigned 64-bit integer</td>
    </tr>
    <tr>
      <td><code>ushort</code>
      </td><td>2</td>
      <td>unsigned 16-bit integer</td>
    </tr>
    <tr>
      <td><code>word</code>
      </td><td>2</td>
      <td>unsigned 16-bit integer</td>
    </tr>
  </tbody>
</table>

#### Attributes
All primitive type elements have the following *optional* attributes.
<!-- Get ready for some ugly HTML! -->
<table>
  <tbody>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Notes</th>
    </tr>
    <tr>
      <td><code>comment</code></td>
      <td>string</td>
      <td>
        <ul>
          <li>intended as an area for keeping notes about the specific data type instance</li>
          <li>content is ignored by template parser</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><code>count</code></td>
      <td>positive integer</td>
      <td>
        <ul>
          <li>number of times to repeat the data type</li>
          <ul><li>a number greater than 1 will create an array</li></ul>
          <li>if attribute is omitted, a value of 1 is assumed</li>
        </ul>
      </td>
    </tr>
    <tr>
      <td><code>name</code></td>
      <td>string</td>
      <td>
        <ul>
          <li>identifier used to refer to the data type instance</li>
          <li>can only contain alphanumeric characters and <code>_</code></li>
          <ul>
            <li>cannot start with a number</li>
            <li>cannot contain <a href="#variables">variables</a></li>
          </ul>
        </ul>
      </td>
    </tr>
    <tr>
      <td><code>sentinel</code></td>
      <td>(varies)</td>
      <td>
        <ul>
          <li>repeats the data type until the value specified by this attribute is found</li>
          <li>similar to <code>count</code>, but used to map arrays without a pre-defined length<br />(e.g. zero-terimated strings)
          <li>the type of this attribute should match the type on which it is being used</li>
        </ul>
      </td>
    </tr>
  </tbody>
</table>

The types `float` and `double` have one extra attribute, which is to be used only in combination with the `sentinel` attribute.
<table>
  <tbody>
    <tr>
      <th>Name</th>
      <th>Type</th>
      <th>Notes</th>
    </tr>
    <tr>
      <td><code>thresh</code></td>
      <td>decimal</td>
      <td>
        <ul>
          <li>specifies a range of values around the sentinel value that will trigger the end of repetition</li>
          <li>accepts values between 0 and 1 (exclusive)</li>
          <li>if attribute is omitted, a value of 0.001 is assumed</li>
        </ul>
      </td>
    </tr>
  </tbody>
</table>

### Structures
(TODO)

### Variables
(TODO)
#### Scope
(TODO)

### Directives
(TODO)

## Examples
(TODO)

## Credits
BFT specification created and maintained by Wes Hampson (2017).
