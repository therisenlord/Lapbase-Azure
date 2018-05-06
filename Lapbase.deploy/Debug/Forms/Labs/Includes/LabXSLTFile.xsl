<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
    <html>
    <body>
    <!--
        This is an XSLT template file. Fill in this area with the
        XSL elements which will transform your XML to XHTML.
    -->
      <table style="width:100%" cellpadding = "0" cellspacing = "0" border="0" class="testNameTable">
        <xsl:for-each select="dsSchema/tblLabs">
          <tr>
			  <xsl:attribute name="onmouseover">
				  javascript:this.style.cursor='pointer';
			  </xsl:attribute>
			  <xsl:attribute name="onmouseout">
				  javascript:this.style.cursor='';
			  </xsl:attribute>
            <td>
				      <xsl:if test='position() mod 2 = 1'>
					      <xsl:attribute name="class">row01</xsl:attribute>
				      </xsl:if>
				      <xsl:if test='position() mod 2 != 1'>
					      <xsl:attribute name="class">row02</xsl:attribute>
				      </xsl:if>
				      <table style="width:100%" cellpadding = "0" cellspacing = "0" border="0">
					      <xsl:attribute name="id">tblLabRow_<xsl:value-of select="PathologyID"/></xsl:attribute>
					      <xsl:attribute name ="onclick">javascript:LabRow_onclick(<xsl:value-of select="PathologyID"/>);</xsl:attribute>

					      <tr>
                  <td style="width:99%">
                    <xsl:attribute name="id">
                      str_LabDate_<xsl:value-of select="PathologyID"/>
                    </xsl:attribute>
                    <xsl:value-of select="str_LabDate"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">
                      Lab_<xsl:value-of select="PathologyID"/>
                    </xsl:attribute>
                    <xsl:value-of select="PathologyID"/>
                  </td>
                </tr>
				      </table>
            </td>
          </tr>
        </xsl:for-each>
      </table>
      
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

