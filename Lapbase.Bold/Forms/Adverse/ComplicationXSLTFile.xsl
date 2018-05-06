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
        <xsl:for-each select="dsSchema/tblComplications">
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
					      <xsl:attribute name="id">tblComplicationRow_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
					      <xsl:attribute name ="onclick">javascript:ComplicationRow_onclick(<xsl:value-of select="tblComplicationsID"/>);</xsl:attribute>

					      <tr>
						      <td style="width:25%">
							      <xsl:attribute name="id">Complication_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <xsl:value-of select="Complication"/>
						      </td>
						      <td style="width:10%">
							      <xsl:attribute name="id">ComplicationCode_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <xsl:value-of select="ComplicationCode"/>
						      </td>
						      <td style="width:15%;display:none">
							      <xsl:attribute name="id">TypeCode_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <xsl:value-of select="TypeCode"/>
						      </td>
						      <td style="width:10%">
							      <xsl:attribute name="id">str_ComplicationDate_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <xsl:value-of select="str_ComplicationDate"/>
						      </td>
						      <td style="width:5%">
							      <xsl:attribute name="id">Readmitted_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <xsl:value-of select="Readmitted"/>
						      </td>
						      <td style="width:5%">
							      <xsl:attribute name="id">ReOperation_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <xsl:value-of select="ReOperation"/>
						      </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">FacilityCode_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
                    <xsl:value-of select="FacilityCode"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">Facility_Other_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
                    <xsl:value-of select="Facility_Other"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">AdverseSurgery_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
                    <xsl:value-of select="AdverseSurgery"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">DoctorID_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
                    <xsl:value-of select="DoctorID"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">
                      DateCreated_<xsl:value-of select="tblComplicationsID"/>
                    </xsl:attribute>
                    <xsl:value-of select="DateCreatedFormated"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">
                      AdmitDate_<xsl:value-of select="tblComplicationsID"/>
                    </xsl:attribute>
                    <xsl:value-of select="str_AdmitDate"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">
                      DischargeDate_<xsl:value-of select="tblComplicationsID"/>
                    </xsl:attribute>
                    <xsl:value-of select="str_DischargeDate"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">
                      PerformDate_<xsl:value-of select="tblComplicationsID"/>
                    </xsl:attribute>
                    <xsl:value-of select="str_PerformDate"/>
                  </td>
                  <td style="width:1%;display:none">
                    <xsl:attribute name="id">
                      Reason_<xsl:value-of select="tblComplicationsID"/>
                    </xsl:attribute>
                    <xsl:value-of select="Reason"/>
                  </td>
                </tr>

					      <xsl:if test ="string-length(Notes) != 0">
					      <tr>
						      <td colspan="10">
							      <xsl:attribute name="id">Notes_<xsl:value-of select="tblComplicationsID"/></xsl:attribute>
							      <span>
								      <xsl:value-of select="Notes"/>
							      </span>
						      </td>
					      </tr>
					      </xsl:if>
				      </table>
            </td>
          </tr>
        </xsl:for-each>
      </table>
      
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

