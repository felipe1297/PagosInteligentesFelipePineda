using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PruebaTecnica.Models;
using System.IO;
using System.Net;
using System.Net.Mail;
using Aspose.Cells;

namespace PruebaTecnica.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            List<Persona> listaPersonas;
            using (var context = new PruebaTecnicaContext())
            {
                listaPersonas = context.Personas.ToList();
            }
            return View(listaPersonas);
        }

        public void email(string mes, string to, string subject)
        {

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress("felipepagosinteligentes@gmail.com");
                SmtpClient smtp = new SmtpClient();
                mail.Subject = subject;
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mail.From.Address, "2021felipepagosinteligentes");
                smtp.Host = "smtp.gmail.com";

                //recipient
                mail.To.Add(new MailAddress(to));

                mail.IsBodyHtml = true;
                string st = mes;

                mail.Body = st;
                smtp.Send(mail);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                Redirect("Index");
            }
        }

        public ActionResult AddCsv()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCsv(string path)
        {
            List<Persona> listPerosona = new List<Persona>();

            if(path != null)
            {
                //Read
                using (var reader = new StreamReader(@path))
                {

                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            Persona persona = new Persona();
                            var line = reader.ReadLine();
                            var values = line.Split(",");

                            //{Nombres},{Apellidos},{Identificacion},{Celular},{Direccion},{Ciudad},{Correo}
                            persona.NombresPersona = values[0];
                            persona.ApellidosPersona = values[1];
                            persona.IdentificacionPersona = values[2];
                            persona.CelularPersona = values[3];
                            persona.DireccionPersona = values[4];
                            persona.CiudadPersona = values[5];
                            persona.CorreoPersona = values[6];

                            listPerosona.Add(persona);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception: " + ex.ToString());
                            return Redirect("Index");
                        }
                    }
                }
            }
            else
            {
                return Redirect("Index");
            }
           

            if (listPerosona.Count != 0) {
                using (var context = new PruebaTecnicaContext())
                {

                    string subject = "Estado almacenamiento de datos";
                    
                        foreach (Persona persona in listPerosona)
                        {
                            try
                            {
                                context.Add(persona);
                                context.SaveChanges();
                                string message = persona.NombresPersona + " " + persona.ApellidosPersona + " datos fueron almacenados correctamente.";
                                email(message, persona.CorreoPersona, subject);
                            }
                            catch (Exception ex)
                            {
                                string message = persona.NombresPersona + " " + persona.ApellidosPersona + " los datos NO fueron almacenados correctamente.";
                                email(message, persona.CorreoPersona, subject);
                            }
                        }
                    
                }
            }

            return Redirect("Index");
        }

        [Route("Persona/Editar/{IdPersona}")]
        public ActionResult Editar(int IdPersona)
        {
            Persona persona = new Persona();
            using (var context = new PruebaTecnicaContext())
            {
                persona = context.Personas.Find(IdPersona);
            }
            return View(persona);
        }

        [Route("Persona/Editar/{IdPersona}")]
        [HttpPost]
        public async Task<ActionResult> Editar(Persona persona)
        {

            try
            {
                using (var context = new PruebaTecnicaContext())
                {
                    Persona oPersona = context.Personas.Find(persona.IdPersona);
                    oPersona.CelularPersona = persona.CelularPersona;
                    oPersona.DireccionPersona = persona.DireccionPersona;
                    oPersona.CiudadPersona = persona.CiudadPersona;

                    await context.SaveChangesAsync();

                    return Redirect("/Persona/Index");
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Excption " + ex);
                return Redirect("/Persona/Index");
            }

        }

        [Route("Persona/Eliminar/{IdPersona}")]
        [HttpGet]
        public async Task<ActionResult> Eliminar(int IdPersona)
        {
            Persona persona = new Persona();
            using (var context = new PruebaTecnicaContext())
            {
                persona = context.Personas.Find(IdPersona);
                context.Personas.Remove(persona);
                await context.SaveChangesAsync();
            }
            return Redirect("/Persona/Index");
        }

        public ActionResult Export()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Export(string path, string typeOfFile)
        {
            List<Persona> listaPersonas;
            using (var context = new PruebaTecnicaContext())
            {
                listaPersonas = context.Personas.ToList();
            }
            if (path != null && listaPersonas.Count > 0)
            {
                if (typeOfFile.Equals("csv"))
                {

                    path = @path + "\\ListadoPersonas.csv";
                    //Write
                    using (var writer = new StreamWriter(@path))
                    {

                        foreach (Persona persona in listaPersonas)
                        {
                            string line = persona.NombresPersona + "," + persona.ApellidosPersona + "," +
                                persona.IdentificacionPersona + "," + persona.CelularPersona + "," +
                                persona.DireccionPersona + "," + persona.CiudadPersona + "," +
                                persona.CorreoPersona;
                            writer.WriteLine(line);
                        }
                    }

                    return Redirect("/Persona/Index");
                }
                else
                {
                    Workbook wb = new Workbook();
                    Worksheet sheet = wb.Worksheets[0];

                    for(int i = 0; i < listaPersonas.Count; i++)
                    {
                        string cellARef = ("A" + ( i + 1 ));
                        Cell cellA = sheet.Cells[cellARef];
                        cellA.PutValue(listaPersonas.ElementAt(i).NombresPersona);
                        string cellBRef = ("B" + (i + 1));
                        Cell cellB = sheet.Cells[cellBRef];
                        cellB.PutValue(listaPersonas.ElementAt(i).ApellidosPersona);
                        string cellCRef = ("C" + (i + 1));
                        Cell cellC = sheet.Cells[cellCRef];
                        cellC.PutValue(listaPersonas.ElementAt(i).IdentificacionPersona);
                        string cellDRef = ("D" + (i + 1));
                        Cell cellD = sheet.Cells[cellDRef];
                        cellD.PutValue(listaPersonas.ElementAt(i).CelularPersona);
                        string cellERef = ("E" + (i + 1));
                        Cell cellE = sheet.Cells[cellERef];
                        cellE.PutValue(listaPersonas.ElementAt(i).DireccionPersona);
                        string cellFRef = ("F" + (i + 1));
                        Cell cellF = sheet.Cells[cellFRef];
                        cellF.PutValue(listaPersonas.ElementAt(i).CiudadPersona);
                        string cellGRef = ("G" + (i + 1));
                        Cell cellG = sheet.Cells[cellGRef];
                        cellG.PutValue(listaPersonas.ElementAt(i).CorreoPersona);


                    }

                    path = @path + "\\ListadoPersonas.xlsx";

                    wb.Save(path, SaveFormat.Xlsx);

                    return Redirect("/Persona/Index");
                }
            }
            else
            {
                return Redirect("Index");
            }

        }

    }
}
