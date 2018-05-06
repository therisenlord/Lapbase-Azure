<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
    <!--
        This is an XSLT template file. Fill in this area with the
        XSL elements which will transform your XML to XHTML.
    -->
      <xsl:for-each select ="dsSchema/tblPatientDocumentItems">
        <table style="width:100%; font-size:small" border="0">
          <tr>
            <td rowspan="3" style="width:88px">
              <div style="vertical-align:top; text-align:center;">
                <div style="border-width:1pt; height:100px; width:88px; border-style:solid; border-color:black; text-align:center;">
                  <xsl:attribute name ="onclick">
                    javascript:tempDIV_onclick(<xsl:value-of select="tblPatientDocumentsID"/>, <xsl:value-of select="IsDeleted = 'true'"/>);
                  </xsl:attribute>
                  <xsl:attribute name ="onmouseover">javascript:this.style.cursor='pointer';</xsl:attribute>
                  <xsl:attribute name ="onmouseout">javascript:this.style.cursor='';</xsl:attribute>
                  <br /><br />
                  <xsl:value-of select="substring-after(DocumentFileName,'.')"/>
                </div>
                <span style="font-size:x-small;text-align:center">
					<!--
                  <a href="#">
                    <xsl:attribute name="onclick">
                      linkFullSize_onclick(<xsl:value-of select="tblPatientDocumentsID"/>);
                    </xsl:attribute>
                    Full-size</a>&#160;&#160;
					-->
                  <a href="#">
                    <xsl:attribute name ="onclick">
                      setDocumentName('<xsl:value-of select="DocumentFileName"/>');tempDIV_onclick(<xsl:value-of select="tblPatientDocumentsID"/>, <xsl:value-of select="IsDeleted = 'true'"/>);
                    </xsl:attribute>
                    Details</a>
                </span>
              </div>
            </td>
            <td> Date : </td>
            <td>
              <xsl:value-of select="EventDate"/> &#160; <xsl:value-of select="EventName"/>
            </td>
          </tr>
          <tr style="vertical-align:top" >
            <td style="width:80px" >Label : </td>
            <td>
              <xsl:value-of select="DocumentName"/>
            </td>
          </tr>
          <tr style="vertical-align:top;" >
            <td >Description : </td>
            <td>
              <textarea rows ="3" readonly="false" style="width:95%; border:0">
                <xsl:value-of select="Doc_Description"/>
              </textarea>
            </td>
          </tr>
        </table>
        <br/>
      </xsl:for-each>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

