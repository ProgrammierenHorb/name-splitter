package org.example.db;

import java.util.Collection;

public interface IDBAccess {

    Collection<String> getAllTitles();

    String getFullTitleByShorthand(String shorthand);
}
