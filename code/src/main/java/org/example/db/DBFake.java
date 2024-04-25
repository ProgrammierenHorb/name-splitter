package org.example.db;

import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class DBFake implements IDBAccess{

    Map<String, String> titles = new HashMap<>();

    public DBFake() {
        titles.put("Mr", "Mister");
        titles.put("Mrs", "Missus");
        titles.put("Dr rer. nat.", "Doktor der Naturwissenschaften");
        titles.put("MdB", "Mitglied des Bundestag");
        titles.put("Prof", "Professor");
        titles.put("Sir", "Sir");
        titles.put("Lady", "Lady");

    }

    @Override
    public Collection<String> getAllTitles() {
        return titles.keySet();
    }

    @Override
    public String getFullTitleByShorthand(String shorthand) {
        return titles.get(shorthand);
    }
}
