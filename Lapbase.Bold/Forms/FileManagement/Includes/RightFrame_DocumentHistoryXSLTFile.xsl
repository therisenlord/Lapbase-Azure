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
      <table style="width:90%">
        <xsl:for-each select="/dsSchema/tblDocumentHistory">
          <tr>
            <td>
              <xsl:value-of select="EventLogName"/>
            </td>
            <td>
              <xsl:value-of select="UserFullName"/>
            </td>
            <td>
              <xsl:value-of select ="EventLogDate"/>
            </td>
            <td>
              <xsl:value-of select="Location"/>
            </td>
            <td>
              <xsl:value-of select="Notes"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

