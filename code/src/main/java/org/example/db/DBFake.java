package org.example.db;

import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class DBFake implements IDBAccess{

    Map<String, String> titles = new HashMap<>();

    @Override
    public Collection<String> getAllTitles() {
        return titles.keySet();
    }

    @Override
    public String getFullTitleByShorthand(String shorthand) {
        return titles.get(shorthand);
    }
}
