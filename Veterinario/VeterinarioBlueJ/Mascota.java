public class Mascota {
    private String numeroCarnet;
    private String nombre;
    private String especie;
    private String raza;
    private int edad;
    private Encargado encargado; // Asociación/Composición

    public Mascota(String numeroCarnet, String nombre, String especie, String raza, int edad, Encargado encargado) {
        this.numeroCarnet = numeroCarnet;
        this.nombre = nombre;
        this.especie = especie;
        this.raza = raza;
        this.edad = edad;
        this.encargado = encargado;
    }

    public String getNumeroCarnet() { return numeroCarnet; }
    public String getNombre() { return nombre; }
    public String getEspecie() { return especie; }
    public String getRaza() { return raza; }
    public int getEdad() { return edad; }
    public Encargado getEncargado() { return encargado; }
}
